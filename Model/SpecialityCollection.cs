using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckClinic.Model
{
    public class SpecialityCollection
    {
        [JsonProperty("changes")]
        public string Changes { get; set; }

        [JsonProperty("response")]
        public IList<Speciality> Specialities { get; set; }
    }
}
