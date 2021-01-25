//using LogApplication.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models.Common.Commands
{
    //ActivityStep.1
    public class EmailCmdParam : CmdParam
    {
        #region Acitiviy Properties
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailTitle { get; set; }
        public string EmailBody { get; set; }
        public string EmailCC { get; set; }
        public bool IsHtmlBody { get; set; }

        public string SmtpHost { get; set; }
        public string FromPassword { get; set; }

        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public NetworkCredential Credentials { get; set; }
        public bool UseDefaultCredentials { get; set; }

        public string ReferenceLink { get; set; }

        #endregion

        public EmailCmdParam(string strSubject, string strBody,  String strTo,
            String strFrom="baicrawler@gmail.com", String strCC = ""
            , bool obMailFormat=true
            ,string smtpHost = "smtp.gmail.com"
            , string fromPassword ="jan06.com"
            ,int port = 587)
        {

            FromPassword=fromPassword ;
            var fromAddress = new MailAddress(strFrom,FromPassword );

           

            CommandName = EnumCommands.Email.ToString();
            EmailFrom = strFrom;
            EmailTo = strTo;
            EmailTitle = strSubject;
            EmailBody = strBody;
            EmailCC = strCC;
            IsHtmlBody = obMailFormat;

            SmtpHost = smtpHost;
            Port = port;
            EnableSsl = true;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            UseDefaultCredentials = false;
            Credentials = new NetworkCredential(fromAddress.Address, FromPassword);


            Payload = this;
            SavePayload();
        }

       public  SmtpClient CreateMailClient()
        {

            FromPassword = FromPassword;
            var fromAddress = new MailAddress(EmailFrom, FromPassword);


            var smtp = new SmtpClient
            {
                Host = SmtpHost,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, FromPassword)
            };
            return smtp;
        }

        public void SavePayload()
        {
            RequestDate = DateTime.Now;
            Payload = this.Payload;
        }
    }

}
