using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.IO.Compression;
using System.IO;
using System.Net.Mime;
using System.Xml.Linq;

namespace SUA.Servicios
{
    public class EmailService
    {
        public EmailCredentials Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public EmailService()
        {
            Credentials = new UserService().GetEmailCredentials();

            Client = new SmtpClient();
            Client.Port = 587;
            Client.Host = "smtp.gmail.com";
            Client.EnableSsl = true;
            Client.Timeout = 10000;
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.UseDefaultCredentials = false;
            Client.Credentials = new System.Net.NetworkCredential(Credentials.Email, Credentials.Password);
        }

        public void SendEmail(string emailTo, string subject, string body, List<string> filesToAttach = null, bool compressFiles = false, string locationBackup = null)
        {
            var mm = new MailMessage(Credentials.Email, emailTo, subject, body);
            if(filesToAttach != null)
            {
                Client.Timeout = 200000;
                if (compressFiles)
                {
                    ////////////////////////////////////////////

                    var zipLocation = locationBackup + "\\SUA-BU-" + DateTime.Now.ToString("yyyy-MM-dd") + ".zip";
                    ZipFile.CreateFromDirectory(locationBackup, zipLocation);
                    mm.Attachments.Add(new Attachment(zipLocation));

                    /////////////////////////////////////////////////
                }
                else
                {
                    foreach (var file in filesToAttach)
                    {
                        mm.Attachments.Add(new Attachment(file));
                    }
                }
            }
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            Client.Send(mm);
        }
    }
}