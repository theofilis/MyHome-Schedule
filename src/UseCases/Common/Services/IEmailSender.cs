using MyHome.Application.Common.Models.Email;
using System.Threading.Tasks;

namespace MyHome.Application.Common.Services
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);

        Task SendEmailAsync(EmailMessage message);
    }
}
