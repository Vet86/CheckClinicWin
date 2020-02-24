using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IClinicCollectionParser
    {
        IList<IClinic> ParseClinics(string content);
    }
}
