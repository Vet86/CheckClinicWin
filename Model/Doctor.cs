using CheckClinic.Interfaces;
using Newtonsoft.Json;

namespace CheckClinic.Model
{
    public class Doctor : IDoctor
    {
        [JsonProperty("IdDoc")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets { get; set; }
    }
}
