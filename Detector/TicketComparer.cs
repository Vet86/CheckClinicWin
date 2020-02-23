using CheckClinic.Interfaces;
using System.Collections.Generic;

namespace CheckClinic.Detector
{
    public class TicketComparer : IEqualityComparer<ITicket>
    {
        public bool Equals(ITicket x, ITicket y)
        {
            return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode(ITicket observeData)
        {
            return observeData.Id.GetHashCode();
        }
    }
}
