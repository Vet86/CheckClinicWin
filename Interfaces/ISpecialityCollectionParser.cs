using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface ISpecialityCollectionParser
    {
        IList<ISpeciality> ParseSpecialities(string content);
    }
}
