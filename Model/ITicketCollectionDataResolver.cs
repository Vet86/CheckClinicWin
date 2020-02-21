namespace CheckClinic.Model
{
    public interface ITicketCollectionDataResolver
    {
        string RequestProcess(string clinicId, string doctorId);
    }
}
