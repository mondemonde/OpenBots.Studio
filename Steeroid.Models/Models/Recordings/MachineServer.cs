using Newtonsoft.Json;
using Steeroid.Models.Enums;
//using SteeroidUpdate.Main.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Steeroid.Models
{
   public class MachineServer:BaseModel
    {
        [IgnoreMember]
        [NotMapped]
        public bool IsDone { get; private set; }


        public void MarkComplete()
        {
            IsDone = true;

            //Events.Add(new MachineRegisterCompletedEvent(this));
        }

        /// <summary>
        /// Local key for opening encrypted message in servicebus mail
        /// also used to check for Nop-QR  handshake
        /// </summary>
        public string PrivateKey { get; set; }


        /// <summary>
        /// make not active 
        /// or flag as deleted
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Max date allowed
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Server side Id of App or Mac
        /// </summary>
        public int GlobalMacId { get; set; }

        /// <summary>
        /// Connection string to use in Azure servive bus
        /// </summary>
        public string ServiceBusConnectionString { get; set; }



        string _topicId;

        /// <summary>
        /// [TopicId] nvarchar(4000) NULL --or entityPATH
        /// used in Azure servive bus connection
        /// ALso known as BusSubscription
        /// </summary>
        public string TopicId
        {
            get
            {
                return _topicId;
            }
            set
            {
                _topicId = value;
            }
        }




        public string BusSubscription
        {
            get
            {
                return _topicId;
            }
            set
            {
                _topicId = value;
            }
        }


        #region ---------NOP-QR-------------------
       
        [JsonProperty("tokenUrl")]
        public string TokenUrl { get; set; }

        [JsonProperty("sendUrl")]
        public string SendUrl { get; set; }



        /// <summary>
        ///  used for authentication
        /// </summary>
        [NotMapped]
        [JsonProperty("sourceId")]
        public string DefaultTopic
        {
            get { return TopicId; }
            set { TopicId = value; }
        }

        /// <summary>
        /// use for sending message
        /// [JsonProperty("loggerName")]
        /// label or title
        /// </summary>
        [JsonProperty("loggerName")]
        public string DefaultLabel { get; set; }


        //"qr_api_mapper.json"              
        //var token = await client.GetQRToken(qrSettings.ClientId, qrSettings.ApiKey, sFileName);

        /// <summary>
        /// [ClientId] nvarchar(4000) NULL
        /// usage:
        /// await client.GetQRToken(qrSettings.ClientId, qrSettings.ApiKey, sFileName);
        /// </summary>
        public string ClientId { get; set; }

        /// <summary> 
        /// use to connect to qr server
        /// usage:
        /// await client.GetQRToken(qrSettings.ClientId, qrSettings.ApiKey, sFileName);
        /// </summary>
        public string APIKey { get; set; }



        #endregion



        public LicenseMode LicenseType { get; set; }

        [NotMapped]
        public int LicenseInt
        {
            get
            {
                return Convert.ToInt32(LicenseType);
            }
            set
            {
                LicenseType = (LicenseMode)value;
            }
        }



        [NotMapped]
        public string LicenseString
        {
            get
            {
                return LicenseType.ToString();
            }
        }

        /// <summary>
        /// shorcut to serialize this object
        /// </summary>
        /// <returns>serialized json of this Machine class</returns>
        public override string ToString()
        {
            this.Modified = this.Created;
            GlobalMacId = Id;

            //encrypt sensivite data..
            //ServiceBusConnectionString =  CryptoEngine.EncryptWithPrivateKey(ServiceBusConnectionString, JwtAuthManager.GetDefaultLocalKey());
            //TopicId = CryptoEngine.EncryptWithPrivateKey(TopicId, JwtAuthManager.GetDefaultLocalKey());

            // return base.ToString();
            var jsonMessage = JsonConvert.SerializeObject(this);
            return jsonMessage;
        }





        /// <summary>
        /// unique company name like website
        /// </summary>
        [Required]
        public string DomainName { get; set; }






    }
}
