using System.Collections.Generic;

namespace CheckClinic.Model
{
    public class TicketCollection
    {
        public IList<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
