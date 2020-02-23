using CheckClinic.Interfaces;

namespace CheckClinic.MailSettings
{
    public class MailSettings : IMailSettings
    {
        public string MailSender => "CheckClinicBot@gmail.com";

        public string PasswordSender => "huvgkmmnuzdcufkq";

        public string NameSender => "CheckClinicBot";

        public string Smtp => "smtp.gmail.com";

        public int Port => 587;
    }
}
