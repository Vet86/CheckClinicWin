using CheckClinic.Model;
using Newtonsoft.Json;

namespace CheckClinic.DataParser
{
    public class DoctorCollectionParser
    {
        public DoctorCollection Parse(string content)
        {
            return JsonConvert.DeserializeObject<DoctorCollection>(content);
        }
    }
}
