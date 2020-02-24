using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IDistrictCollectionParser
    {
        IList<IDistrict> ParseDistricts(string html);
    }
}
