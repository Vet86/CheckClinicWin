using CheckClinic.Interfaces;
using System.Collections.Generic;

namespace CheckClinic.Detector
{
    class MailTextPreparer
    {
        public MailTextPreparer(IObserveData observeData, IEnumerable<ITicket> newTickets)
        {
            //Title = $"{observeData.} новые номерки";
        }

        public string Title { get; }
        public string Content { get; }
    }
}
