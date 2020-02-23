using CheckClinic.Interfaces;
using System;

namespace CheckClinic.Detector
{
    class MailSettings : IMailSettings
    {
        public string MailSender => "CheckClinicBot@gmail.com";

        public string PasswordSender => "huvgkmmnuzdcufkq";

        public string NameSender => "CheckClinicBot";

        public string SendEmail(string content)
        {
            throw new NotImplementedException();
        }
    }
}
