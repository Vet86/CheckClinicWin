﻿using System;
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
        public string MailReceiver { get; set; } = "vitalyev_aleksey@mail.ru";
        public async Task SendEmailAsync(ResponseDoctorModel responseDoctor)
        {
            MailAddress from = new MailAddress(MailSender, NameSender);
            MailAddress to = new MailAddress(MailReceiver);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"{responseDoctor.DoctorName} новые номерки";
            m.Body = $"У доктора {responseDoctor.DoctorName} {responseDoctor.FreeTickets} номерков";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(MailSender, PasswordSender),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,                
            };
            await smtp.SendMailAsync(m);
        }
    }
}
