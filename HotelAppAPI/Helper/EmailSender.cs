using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using HotelAppAPI.Helper;
using Newtonsoft.Json.Linq;
using Mailjet.Client.Resources;

namespace HotelAppAPI.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly MailJetSetting mailjetSettings;
        public EmailSender(IOptions<MailJetSetting> mailjetSettings)
        {
            this.mailjetSettings = mailjetSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(mailjetSettings.ApiKey, mailjetSettings.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", mailjetSettings.Email},
        {"Name", "HotelApp1"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       htmlMessage
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }

}
