using CheckClinic.Model;
using System;
using System.Collections.Generic;

namespace CheckClinic.Detector
{
    public class ObserveDataComparer : IEqualityComparer<IObserveData>
    {
        public bool Equals(IObserveData x, IObserveData y)
        {
            return string.Equals(x.ClinicId, y.ClinicId) && string.Equals(x.DoctorId, y.DoctorId);
        }

        public int GetHashCode(IObserveData observeData)
        {
            return observeData.ClinicId.GetHashCode() ^ observeData.DoctorId.GetHashCode();
        }
    }
}
