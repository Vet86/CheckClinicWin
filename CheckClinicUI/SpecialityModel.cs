using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckClinicUI
{
    public class SpecialityModel
    {
        [JsonProperty("response")]
        public IList<ResponseDoctorModel> ResponseModels { get; set; }
    }

    public class ResponseDoctorModel
    {
        [JsonProperty("Name")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets { get; set; }
    }
}
