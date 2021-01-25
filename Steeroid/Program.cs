using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeroid._Infra;
using Steeroid.Business.Areas.Communication.WebApi;
using Steeroid.Business.Machine;
using Steeroid.Business.Services;
using Steeroid.Infra2._0;
using Steeroid.Infra2._0.Areas.Machine;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;

namespace Steeroid
{
    public class Program
    {
        public static void Main(string[] args)
        {
           StartupSetup.MyHost = CreateHostBuilder(args).Build();          

           StartupSetup.MyHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls(DefaultApiPort.GetLocalBaseAddress((DefaultApiPort.MainPort))) //(@"http://localhost:9321/")
                        .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                            
                            // logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure
                        });


                });


    }
}
