using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckClinic.Model
{
    public class DoctorCollection
    {
        [JsonProperty("response")]
        public IList<Doctor> Doctors { get; set; }

        public int Id { get; set; }
    }
}
