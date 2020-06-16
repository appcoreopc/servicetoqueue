using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

class MailManager {

 private readonly SmtpClient _client; 

 public MailManager(SmtpClient client)
 {
    this._client = client; 
 }
 
 public async Task SendMail() 
 {

         MailAddress from = new MailAddress("jane@contoso.com",
               "Jane " + (char)0xD8+ " Clayton",
            System.Text.Encoding.UTF8);

           MailAddress to = new MailAddress("ben@contoso.com");
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
            _client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string userState = "test message1";
             _client.SendAsync(message, userState);
    }

    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        throw new NotImplementedException();
    }
}

class MailHelper
{
    public static SmtpClient GetClient(SmtpHost smtpHost) {
     return new SmtpClient(smtpHost.HostName, smtpHost.Port);
 }
}

public class SmtpHost {
    public string HostName { get; set; }

    public int Port { get; set; }
}