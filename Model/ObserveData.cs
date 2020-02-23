using CheckClinic.Interfaces;

namespace CheckClinic.Model
{
    public class ObserveData : IObserveData
    {
        public ObserveData(string clinicId, string doctorId)
        {
            ClinicId = clinicId;
            DoctorId = doctorId;
        }

        public string ClinicId { get; set; }

        public string DoctorId { get; set; }
    }
}
