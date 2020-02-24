using CheckClinic.Interfaces;
using CheckClinic.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CheckClinic.DataParser
{
    public class ClinicCollectionParser : IClinicCollectionParser
    {
        public IList<IClinic> ParseClinics(string content)
        {
            return JsonConvert.DeserializeObject<ClinicCollection>(content).ClinicModels.Cast<IClinic>().ToList();
        }
    }
}
