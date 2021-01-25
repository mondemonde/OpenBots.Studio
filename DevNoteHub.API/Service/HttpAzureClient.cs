using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using QR.API.Mapper;
using QR.API.Mapper.Std;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Steeroid.Models.Enums;

namespace DevNoteHub.API.Service
{
    public class HttpAzureClient
    {


        #region Fields

        //https://beta.quickreach.co/gateway/api/v2/board/external/nopc?sourceId=05475304-e149-4417-8a7a-3cef790c6ae4&loggerName=Beta Ticket Test
        public string REQUEST_URL = "https://beta.quickreach.co/gateway/api/v2/board/external/nopc?";
        public string TOKEN_URL = "https://beta.quickreach.co/identity/Connect/Token";
        public const string QR_VERSION = "0.1";

        public int MaxReqCount = -1;

        //Internal request counter. Max requests = 500 per session
        private int _requestCount;
        private int? _domainHash;

        #endregion

        // public async Task<string> SendEvent(OrderQR order,string eventTag)
        public async Task<HttpResponseMessage> GetQRToken(string userName, string password,string jsonPathConfig = "qr_api_mapper.json")
        {
           // string sFileName = HttpContext.Current.Server.MapPath(@"~/dirname/readme.txt");

            var setting = LoadJson<JsonSettingQr>(jsonPathConfig);

            using (var client = new HttpClient())
            {
                var webUrl = setting.TokenUrl;

                var uri = string.Empty;//"connect/token";
                client.BaseAddress = webUrl;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.ConnectionClose = true;

                //Set Basic Auth
                //var user = "username";
                //var password = "password";
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

                // Populate the form variable
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var formContent = new FormUrlEncodedContent(formVariables);

                //var result = await client.PostAsJsonAsync(uri, );
                var result = await client.PostAsync(uri, formContent);


                return result;
            }




        }


        //_STEP.PUBLISHER  #2  PostBusMessage(Message message)
        public async Task<HttpResponseMessage> PostBusMessage(Message message)
        {
            var setting_SendUrl = message.UserProperties[MSG.fx_setting_SendUrl.ToString()];
            var setting_Code = message.UserProperties[MSG.fx_setting_Code.ToString()];



            using (var client = new HttpClient())
            {
                var webUrl = setting_SendUrl.ToString();
                //webUrl = $"https://live.quickreach.co/gateway/api/v2/board/external/nopc?sourceId={{sourId}}&loggerName={{loggerName}}";

                //webUrl = webUrl.Replace("##code##",setting_Code.ToString());
                //webUrl = webUrl.Replace("##loggerName##", eventLabel);

                webUrl = string.Format("{0}?code={1}", setting_SendUrl, setting_Code);
                //webUrl = @"http://localhost:7071/api/AutoPlay";

                var uri = string.Empty;//"connect/token";

                client.BaseAddress =new Uri(webUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.ConnectionClose = true;

                //Set token Auth               
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                /////////////////////////////////////////////HACK------------------------------------
                //var message = new Message();
                //message.TimeToLive = TimeSpan.FromSeconds(10);
                var systemProperties = new Message.SystemPropertiesCollection();

                // systemProperties.EnqueuedTimeUtc = DateTime.UtcNow.AddMinutes(1);
                var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
                var value = DateTime.UtcNow.AddMinutes(1);
                systemProperties.GetType().InvokeMember("EnqueuedTimeUtc", bindings, Type.DefaultBinder, systemProperties, new object[] { value });
                // workaround "ThrowIfNotReceived" by setting "SequenceNumber" value
                systemProperties.GetType().InvokeMember("SequenceNumber", bindings, Type.DefaultBinder, systemProperties, new object[] { 1 });

                // message.systemProperties = systemProperties;
                bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
                message.GetType().InvokeMember("SystemProperties", bindings, Type.DefaultBinder, message, new object[] { systemProperties });
                ////////////////////////////////////end HACK----------------------------------------


                var myContent = JsonConvert.SerializeObject(message);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(uri, byteContent);


                return result;
            }




        }


        public async Task<HttpResponseMessage> PostBusMessage(Message message,
            string setting_SendUrl, string setting_Code)
        {        

            using (var client = new HttpClient())
            {
                var webUrl = setting_SendUrl.ToString();
                //webUrl = $"https://live.quickreach.co/gateway/api/v2/board/external/nopc?sourceId={{sourId}}&loggerName={{loggerName}}";

                //webUrl = webUrl.Replace("##code##",setting_Code.ToString());
                //webUrl = webUrl.Replace("##loggerName##", eventLabel);

                webUrl = string.Format("{0}?code={1}", setting_SendUrl, setting_Code);
                //webUrl = @"http://localhost:7071/api/AutoPlay";

                var uri = string.Empty;//"connect/token";

                client.BaseAddress = new Uri(webUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.ConnectionClose = true;

                //Set token Auth               
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                /////////////////////////////////////////////HACK------------------------------------
                //var message = new Message();
                //message.TimeToLive = TimeSpan.FromSeconds(10);
                var systemProperties = new Message.SystemPropertiesCollection();

                // systemProperties.EnqueuedTimeUtc = DateTime.UtcNow.AddMinutes(1);
                var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
                var value = DateTime.UtcNow.AddMinutes(1);
                systemProperties.GetType().InvokeMember("EnqueuedTimeUtc", bindings, Type.DefaultBinder, systemProperties, new object[] { value });
                // workaround "ThrowIfNotReceived" by setting "SequenceNumber" value
                systemProperties.GetType().InvokeMember("SequenceNumber", bindings, Type.DefaultBinder, systemProperties, new object[] { 1 });

                // message.systemProperties = systemProperties;
                bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
                message.GetType().InvokeMember("SystemProperties", bindings, Type.DefaultBinder, message, new object[] { systemProperties });
                ////////////////////////////////////end HACK----------------------------------------


                var myContent = JsonConvert.SerializeObject(message);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(uri, byteContent);


                return result;
            }




        }


        public static T LoadJson<T>(string jsonPath)
        {



            using (StreamReader r = new StreamReader(jsonPath))
            {
                string json = r.ReadToEnd();
                var item = JsonConvert.DeserializeObject<T>(json);
                return item;
            }
        }

    }

  

}
