using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckClinicUI
{
    public class ClinicModel
    {
        [JsonProperty("changes")]
        public string Changes { get; set; }

        [JsonProperty("response")]
        public IList<ResponseModel> ResponseModels { get; set; }
    }

    public class ResponseModel
    {
        [JsonProperty("IdSpesiality")]
        public int Id { get; set; }

        [JsonProperty("NameSpesiality")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets { get; set; }
    }
}
