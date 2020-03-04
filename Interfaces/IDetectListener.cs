using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IDetectListener
    {
        void NewTicketsAdded(IObserveData observeData, IEnumerable<ITicket> newTickets);
    }
}
