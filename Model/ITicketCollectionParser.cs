namespace CheckClinic.Model
{
    public interface ITicketCollectionParser
    {
        TicketCollection Parse(string content);
    }
}
