namespace MapProject.WebUI.Services.EmailService
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Password { get; set; }
        public string ReceiverEmail { get; set; }  // Formun gideceği e-posta adresi
    }
}
