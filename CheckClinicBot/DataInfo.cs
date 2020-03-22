using CheckClinic.Interfaces;

namespace CheckClinic.Bot
{
    internal class DataInfo
    {
        public IDistrict District { get; set; }
        public IClinic Clinic { get; set; }
        public ISpeciality Speciality { get; set; }
        public IDoctor Doctor { get; set; }
    }
}
