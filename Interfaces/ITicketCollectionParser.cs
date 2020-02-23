using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface ITicketCollectionParser
    {
        IReadOnlyList<ITicket> Parse(string content);
    }
}
