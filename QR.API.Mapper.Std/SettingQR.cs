using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace QR.API.Mapper
{
    public partial class JsonSettingQr
    {
        [JsonProperty("tokenUrl")]
        public Uri TokenUrl { get; set; }

        [JsonProperty("sendUrl")]
        public Uri SendUrl { get; set; }

        /// <summary>
        ///  [JsonProperty("sourceId")]
        /// </summary>
        [JsonProperty("sourceId")]
        public string DefaultTopic { get; set; }

        [JsonProperty("serviceBusConnectionString")]
        public string ServiceBusConnectionString { get; set; }

        //"entityPath": "qr_integration_topic",
        [JsonProperty("entityPath")]
        public string EntityPath { get; set; }



        //"subscriptionName": "QRIntegrationBot",
        [JsonProperty("subscriptionName")]
        public string SubscriptionName { get; set; }


        /// <summary>
        /// [JsonProperty("loggerName")]
        /// </summary>
        [JsonProperty("loggerName")]
        public string DefaultLabel { get; set; }

        [JsonProperty("exclude")]
        public string[] Exclude { get; set; }


    }

}

