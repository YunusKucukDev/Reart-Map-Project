namespace MapProject.WebUI.Services.EmailService
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(string fromName, string fromEmail, string subject, string message);
    }
}
