using CheckClinic.Interfaces;
using Newtonsoft.Json;

namespace CheckClinic.Model
{
    public class Clinic : IClinic
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("LPUFullName")]
        public string FullName { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }
    }
}
