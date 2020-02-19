using CheckClinic.Model;
using Newtonsoft.Json;

namespace CheckClinic.DataParser
{
    public class SpecialityCollectionParser
    {
        public SpecialityCollection ParseSpecialities(string content)
        {
            return JsonConvert.DeserializeObject<SpecialityCollection>(content);
        }
    }
}
