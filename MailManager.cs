    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Threading.Tasks;

    class MailManager {

    private readonly SmtpClient _client; 
    private readonly Action<object, AsyncCompletedEventArgs> _action; 

    public MailManager(SmtpClient client, Action<object, AsyncCompletedEventArgs> action)
    {
        this._client = client; 
        this._action = action; 
    }
    
    public async Task SendMailAsync() 
    {
            MailAddress from = new MailAddress("jeremy.woo@adaptiv.co.nz",
                "Jeremy", System.Text.Encoding.UTF8);

            MailAddress to = new MailAddress("kepung@gmail.com");
                // Specify the message content.
                MailMessage message = new MailMessage(from, to);

                message.Body = "This is a test email message sent by an application. ";
                // Include some non-ASCII characters in body and subject.
                string someArrows = new string(new char[] {'\u2190', '\u2191', '\u2192', '\u2193'});
                message.Body += Environment.NewLine + someArrows;
                message.BodyEncoding =  System.Text.Encoding.UTF8;
                message.Subject = "test message 1" + someArrows;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // Set the method that is called back when the send operation ends.
                
                if (this._action != null)
                {
                    _client.SendCompleted += new SendCompletedEventHandler(this._action);
                }

                string userState = "test message1";
                 
                Console.WriteLine("Sendng mesages");
                 _client.SendAsync(message, userState);

             await Task.Delay(4000);
        }

    }

    class MailHelper
    {
        public static SmtpClient GetClient(SmtpHost smtpHost) 
        {
            return new SmtpClient(smtpHost.HostName, smtpHost.Port);
        }
    }

    public class SmtpHost {
        public string HostName { get; set; }

        public int Port { get; set; }
    }