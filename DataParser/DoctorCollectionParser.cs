using CheckClinic.Interfaces;
using CheckClinic.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CheckClinic.DataParser
{
    public class DoctorCollectionParser : IDoctorCollectionParser
    {
        public IList<IDoctor> ParseDoctors(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<DoctorCollection>(content).Doctors.Cast<IDoctor>().ToList();
            }
            catch
            {
                return new List<IDoctor>();
            }
        }
    }
}
