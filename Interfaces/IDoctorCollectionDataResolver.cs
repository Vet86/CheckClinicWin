using System.Threading.Tasks;

namespace CheckClinic.Interfaces
{
    public interface IDoctorCollectionDataResolver
    {
        string RequestProcess(string clinicId, string specialitiId);
        Task<string> RequestProcessAsync(string clinicId, string specialitiId);
    }
}
