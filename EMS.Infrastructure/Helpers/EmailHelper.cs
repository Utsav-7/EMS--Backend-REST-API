using System.Net.Mail;
using System.Net;
using EMS_Backend_Project.EMS.Application.Interfaces;

namespace EMS_Backend_Project.EMS.Infrastructure.Services
{
    public class EmailHelper : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SenderEmail"],
                        _configuration["EmailSettings:SenderPassword"]
                    ),
                    EnableSsl = true
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"✅ Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Error: {ex.Message}");
            }
        }

        private const string ContainerEnd = @"
                <div style='text-align: center; margin-top: 30px;'>
                    <p style='color: #7f8c8d; font-size: 14px;'>Best Regards,</p>
                    <p style='color: #2c3e50; font-size: 16px; font-weight: bold;'>Bacancy - EMS Team</p>
                    <img src='https://drive.google.com/uc?export=view&id=1J-OM4zr7GXGMuI6Hzl6pT8HOsAL98z5n' alt='Bacancy Logo' style='width: 60%; margin-top: 10px;' />
                </div>
            </div>";


        public async Task SendUserRegistrationEmailAsync(string toEmail, string? password)
        {
            string emailSubject = "Welcome to Our Platform!";
            string emailBody = $@"
                                <h2 style='color: #2c3e50; text-align: center;'>Welcome to Bacancy - EMS</h2>
                                <p style='font-size: 20px; color: #333; text-align: center;'>
                                    Your account has been successfully registered.
                                </p>
                                <div style='background-color: #ffffff; padding: 15px; border-radius: 8px; 
                                            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); text-align: center;'>
                                    <p style='font-size: 16px;'><strong>Email:</strong> {toEmail}</p>
                                    <p style='font-size: 16px;'><strong>Password:</strong> {password}</p>
                                    <p style='color: #e74c3c;'><em>Please change your password after logging in.</em></p>
                                </div>
                                <div style='text-align: center; margin-top: 20px;'>
                                    <a href='https://yourwebsite.com/login' 
                                       style='background-color: #248f24; color: #ffffff; padding: 12px 20px; 
                                              text-decoration: none; border-radius: 5px; font-size: 16px; font-weight: bold;'>
                                        Login Now
                                    </a>
                                </div>
                            {ContainerEnd}";

            await SendEmailAsync(toEmail, emailSubject, emailBody);
        }
    }
}