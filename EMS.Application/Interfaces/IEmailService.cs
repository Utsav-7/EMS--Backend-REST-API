namespace EMS_Backend_Project.EMS.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendUserRegistrationEmailAsync(string toEmail, string password);
    }
}