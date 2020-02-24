using CheckClinic.Interfaces;
using CheckClinic.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CheckClinic.DataParser
{
    public class SpecialityCollectionParser : ISpecialityCollectionParser
    {
        public IList<ISpeciality> ParseSpecialities(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<SpecialityCollection>(content).Specialities.Cast<ISpeciality>().ToList();
            }
            catch
            {
                return new List<ISpeciality>();
            }
        }
    }
}
