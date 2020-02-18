using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckClinic.Model
{
    public class ClinicCollection
    {
        [JsonProperty("changes")]
        public string Changes { get; set; }

        [JsonProperty("response")]
        public IList<Clinic> ClinicModels { get; set; }
    }
}
