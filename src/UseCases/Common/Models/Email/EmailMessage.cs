using System.Collections.Generic;

namespace MyHome.Application.Common.Models.Email
{
    public class EmailMessage
    {
        public EmailMessage(IEnumerable<string> to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }

        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
