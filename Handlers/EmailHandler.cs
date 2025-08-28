
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security;
using Dapper;
using System.Text;
using System.Text.Json;
using AMSDataLoad.Data;
using AMSDataLoad.Models;
using System.Security.Cryptography.X509Certificates;

namespace AMSDataLoad.Handlers
{
    public class EmailHandler
    {
        private readonly DataContextDapper _dapper;
        public EmailHandler(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        public bool SendEmail(String subject, String body)
        {
            /*
            string emailUrl = "EmailLink";

                EmailToSend emailToSend = new EmailToSend()
                {
                    EmailBody = 
                    EmailSubject = ,
                    EmailTo = 
                };

                string jsonContent = JsonSerializer.Serialize(emailToSend);

                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiToken);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = client.PostAsync(new Uri(emailUrl), content).Result;

                Console.WriteLine(JsonSerializer.Serialize(response));
                */

            EmailNotification Notification = new EmailNotification();
            Notification.EmailBody = "Error in AgencyZoom Load: " + body; // +
                                                                      //System.Environment.NewLine + "Please visit http://SupplyChain.ReadingBody.com/Site-Permission to grant access.";
            Notification.EmailSubject = subject;
            Notification.RequestType = "AgencyZoom Load";

            if (this._dapper.ExecuteSendEmail(Notification, "dkrepps@abeerconsulting.com") > 0)
            {
                return true;
            }
            return false;
        }

    }
}
