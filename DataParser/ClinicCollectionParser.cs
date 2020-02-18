using CheckClinic.Model;
using Newtonsoft.Json;

namespace CheckClinic.DataParser
{
    public class ClinicCollectionParser
    {
        public ClinicCollection ParseClinics(string content)
        {
            return JsonConvert.DeserializeObject<ClinicCollection>(content);
        }
    }
}
