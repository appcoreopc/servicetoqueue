using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace servicebuswriter
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://devsbbank.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZeoCedTJSaqVPAx8bHX998DVIYHtuG5g0OKlUkUFF9g=";
        const string QueueName = "devsbqbank";
        static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            string to = "jeremy.woo@adaptiv.co.nz";
            string from = "jeremy.woo@adaptiv.co.nz";
            string subject = "Using the new SMTP client.";
            string body = @"Using this new feature, you can send an email message from an application very easily.";

            MailMessage message = new MailMessage(from, to, subject, body);

            var server = "smtp.socketlabs.com";

            SmtpClient client = new SmtpClient(server, 2525);
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.Credentials = new NetworkCredential("server33448", "y8P7Ywk6DKn5x9S2");
            client.Send(message);

            Console.WriteLine("Done!");

            var smtpClient = new SmtpClient("smtp.socketlabs.com", 25);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("server33448", "y8P7Ywk6DKn5x9S2");

            var mailManager = new MailManager(smtpClient, (s, e) =>
            {

                Console.WriteLine("message handler");

                String token = (string)e.UserState;

                if (e.Cancelled)
                {
                    Console.WriteLine("[{0}] Send canceled.", token);
                }
                if (e.Error != null)
                {
                    Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                }
                else
                {
                    Console.WriteLine("Message sent.");
                }
            });


            //            var client = new RestClient("https://inject.socketlabs.com/api/v1/email");
            // var request = new RestRequest(Method.POST);
            // request.AddHeader("content-type", "application/json");
            // request.AddParameter("application/json", "{\"serverId\":\"string (required)\",\"APIKey\":\"string (required)\",\"Messages\":[{\"To\":[{\"emailAddress\":\"string (required)\",\"friendlyName\":\"string (optional)\"}],\"From\":{\"emailAddress\":\"string (required)\",\"friendlyName\":\"string (optional)\"},\"ReplyTo\":{\"emailAddress\":\"string (required)\",\"friendlyName\":\"string (optional)\"},\"Subject\":\"string (required)\",\"TextBody\":\"string (optional)\",\"HtmlBody\":\"string (optional)\",\"ApiTemplate\":\"string (optional)\",\"MessageId\":\"string (optional)\",\"MailingId\":\"string (optional)\",\"Charset\":\"string (optional)\",\"CustomHeaders\":[{\"Name\":\"string (optional)\",\"Value\":\"string (optional)\"}],\"CC\":[{\"emailAddress\":\"string (optional)\",\"friendlyName\":\"string (optional)\"}],\"BCC\":[{\"emailAddress\":\"string (optional)\",\"friendlyName\":\"string (optional)\"}],\"Attachments\":[{\"Name\":\"string (optional)\",\"Content\":\"string (optional)\",\"ContentId\":\"string (optional)\",\"ContentType\":\"string (optional)\",\"CustomHeaders\":[{\"Name\":\"string (optional)\",\"Value\":\"string (optional)\"}]}],\"MergeData\":{\"PerMessage\":[[{\"Field\":\"string (optional)\",\"Value\":\"string (optional)\"}]],\"Global\":[{\"Field\":\"string (optional)\",\"Value\":\"string (optional)\"}]}}]}", ParameterType.RequestBody);
            // IRestResponse response = client.Execute(request);


            Console.WriteLine("Sending emails ");
            await mailManager.SendMailAsync();




            var d = new DataMessage
            {
                Name = "jeremy" + DateTime.Now,
                Email = "kepung@gmail.com"
            };

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            var qms = new QueueMessageSender(queueClient);
            await qms.SendMessagesAsync(MessageConverter.Serialize(d));
            Console.WriteLine($"Send message. {d.Name}");
            await queueClient.CloseAsync();
        }
    }

    public class DataMessage
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
