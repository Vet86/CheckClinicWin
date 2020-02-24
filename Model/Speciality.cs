using CheckClinic.Interfaces;
using Newtonsoft.Json;

namespace CheckClinic.Model
{
    public class Speciality : ISpeciality
    {
        [JsonProperty("IdSpesiality")]
        public string Id { get; set; }

        [JsonProperty("NameSpesiality")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets { get; set; }
    }
}
