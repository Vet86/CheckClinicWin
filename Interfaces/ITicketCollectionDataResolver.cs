namespace CheckClinic.Interfaces
{
    public interface ITicketCollectionDataResolver
    {
        string RequestProcess(string clinicId, string doctorId);
    }
}
