using Newtonsoft.Json;

namespace CheckClinic.Model
{
    public class Clinic
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("LPUFullName")]
        public string FullName { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }
    }
}
