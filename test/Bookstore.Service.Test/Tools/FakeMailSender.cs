using System.Collections.Generic;

namespace Bookstore.Service.Test
{
    public class FakeMailSender : IMailSender
    {
        public List<Mail> MailLog { get; set; } = new List<Mail>();

        public void SendMail(string message, List<string> emailAddresses)
        {
            foreach (var email in emailAddresses)
                MailLog.Add(new Mail { Message = message, ToEmailAddress = email });
        }

        public class Mail
        {
            public string Message { get; set; }
            public string ToEmailAddress { get; set; }
        }
    }
}