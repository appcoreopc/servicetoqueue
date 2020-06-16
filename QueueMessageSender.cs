using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

class QueueMessageSender {
    private readonly IQueueClient _queueClient; 

    public QueueMessageSender(IQueueClient queueClient)
    {
        this._queueClient = queueClient;
    }

    public async Task SendMessagesAsync(string messageSource)
    {
        try
        {
            var message = new Message(Encoding.UTF8.GetBytes(messageSource));
            await this._queueClient.SendAsync(message);
        }
        catch (Exception exception)
        {
            
        }
     }
}