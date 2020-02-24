namespace CheckClinic.Interfaces
{
    public interface IDoctorCollectionDataResolver
    {
        string RequestProcess(string clinicId, string specialitiId);
    }
}
