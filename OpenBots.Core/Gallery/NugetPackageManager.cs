﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet;
using OpenBots.Core.Gallery.Models;
using HttpClient = System.Net.Http.HttpClient;

namespace OpenBots.Core.Gallery
{
    public class NugetPackageManger
    {
        readonly Uri nugetV3FeedUri;
        Feed feed;

        public enum PackageType
        {
            Automation,
            Command
        }

        public NugetPackageManger(Uri nugetV3FeedUri = null)
        {
            if (nugetV3FeedUri == null)
            {
                nugetV3FeedUri = new Uri("https://dev.gallery.openbots.io/v3/index.json");
            }
            this.nugetV3FeedUri = nugetV3FeedUri;

            ServicePointManager.SecurityProtocol |= (SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
                                             SecurityProtocolType.Tls);
            ServicePointManager.DefaultConnectionLimit = 10;
        }

        public async Task<SemanticVersion> GetLatestPackageVersionAsync(string packageId, CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                feed = await GetJson<Feed>(nugetV3FeedUri, httpClient, token);
                var searchQueryService = feed.Resources.FirstOrDefault(x => x.Type == "SearchQueryService");

                var searchPackageUri = new Uri($"{searchQueryService.Url}/?q={packageId}");
                var searchResult = await GetJson<SearchResult>(searchPackageUri, httpClient, token);
                var searchResultPackage = searchResult.Data.FirstOrDefault();
                return new SemanticVersion(searchResultPackage.Version);
            }
        }

        public async Task<List<SearchResultPackage>> GetAllLatestPackagesAsync(PackageType type, CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                feed = await GetJson<Feed>(nugetV3FeedUri, httpClient, token);
                var searchQueryService = feed.Resources.FirstOrDefault(x => x.Type == "SearchQueryService");

                var searchPackageUri = new Uri($"{searchQueryService.Url}/{type}");
                var searchResult = await GetJson<SearchResult>(searchPackageUri, httpClient, token);
                return searchResult.Data;
            }
        }

        public async Task<List<RegistrationItem>> GetPackageRegistrationAsync(string packageId, CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                feed = await GetJson<Feed>(nugetV3FeedUri, httpClient, token);
                var registrationService = feed.Resources.FirstOrDefault(x => x.Type == "RegistrationsBaseUrl");

                var registrationUri = new Uri($"{registrationService.Url}/{packageId}/index.json");
                var registration = await GetJson<Registration>(registrationUri, httpClient, token);
                return registration.Items;
            }
        }

        public async Task DownloadPackageAsync(string packageId, SemanticVersion version, string destinationPath, string packageFileName, CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                if (feed == null)
                    feed = await GetJson<Feed>(nugetV3FeedUri, httpClient, token);

                var packageBaseAddress = feed.Resources.FirstOrDefault(x => x.Type.StartsWith("PackageBaseAddress"));
                var packageBaseAddressUri = new Uri($"{packageBaseAddress.Url}/{packageId.ToLower()}/{version}/{packageId.ToLower()}.{version}.nupkg");

                var response = await httpClient.GetAsync(packageBaseAddressUri, token).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var fileExtension = Path.GetExtension(packageBaseAddressUri.Segments.Last());

                var path = Path.Combine(destinationPath, packageFileName + fileExtension);
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }
        }

        private async Task<T> GetJson<T>(Uri uri, HttpClient httpClient, CancellationToken token)
        {
            var response = await httpClient.GetAsync(uri, token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
