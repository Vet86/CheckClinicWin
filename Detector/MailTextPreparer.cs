﻿using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckClinic.Detector
{
    public class MailTextPreparer
    {
        public MailTextPreparer(IObserveData observeData, IReadOnlyList<ITicket> newTickets, string doctorName)
        {
            var ticketsCnt = newTickets.Count;
            var textDoctorName = doctorName ?? observeData.DoctorId;
            Title = $"{textDoctorName} - новых номерков {newTickets.Count}";
            var stringBuilder = new StringBuilder();
            foreach(var ticket in newTickets)
            {
                stringBuilder.AppendLine(ticket.Time);
            }
            Content = stringBuilder.ToString();
            FullMessage = $"{Title}{Environment.NewLine}{Content}";
        }

        public string Title { get; }
        public string Content { get; }
        public string FullMessage { get; }
    }
}
