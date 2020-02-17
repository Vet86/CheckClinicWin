using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CheckClinicUI
{
    class MailNotifier
    {
        public string MailSender { get; set; } = "CheckClinicBot@gmail.com";
        public string PasswordSender { get; set; } = "huvgkmmnuzdcufkq";
        public string NameSender { get; set; } = "CheckClinicBot";
        public string MailReceiver { get; set; } = "vitalyev_aleksey@mail.ru;vitaleva_julia@mail.ru";
        public async Task SendEmailAsync(ResponseDoctorModel responseDoctor)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(MailSender, NameSender)
            };

            foreach (var address in MailReceiver.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(address);
            }

            mailMessage.Subject = $"{responseDoctor.DoctorName} новые номерки";
            mailMessage.Body = $"У доктора {responseDoctor.DoctorName} {responseDoctor.FreeTickets} номерков";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(MailSender, PasswordSender),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };
            await smtp.SendMailAsync(mailMessage);
        }

        internal async Task SendTestMailAsync()
        {
            var responseDoctor = new ResponseDoctorModel()
            {
                DoctorName = "Test doctor",
                FreeTickets = 7
            };
            await SendEmailAsync(responseDoctor);
        }
    }
}
