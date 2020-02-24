using CheckClinic.Interfaces;

namespace CheckClinic.Model
{
    public class District : IDistrict
    {
        public District(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
