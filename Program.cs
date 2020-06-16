using System;
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
            var d = new DataMessage {
                Name  = "jeremy" + DateTime.Now, 
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
