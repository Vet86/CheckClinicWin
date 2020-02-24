using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IDoctorCollectionParser
    {
        IList<IDoctor> ParseDoctors(string content);
    }
}
