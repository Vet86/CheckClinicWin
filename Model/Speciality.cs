using Newtonsoft.Json;

namespace CheckClinic.Model
{
    public class Speciality
    {
        [JsonProperty("IdSpesiality")]
        public int Id { get; set; }

        [JsonProperty("NameSpesiality")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets { get; set; }
    }
}
