using CheckClinic.Interfaces;

namespace CheckClinic.Model
{
    public class ObserveData : IObserveData
    {
        public ObserveData(string clinicId, string doctorId, string doctorName)
        {
            ClinicId = clinicId;
            DoctorId = doctorId;
            DoctorName = doctorName;
        }

        public string ClinicId { get; set; }

        public string DoctorId { get; set; }

        public string DoctorName { get; set; }
    }
}
