using Autofac;
using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckClinic.Bot
{
    class Repository
    {
        private class ClinicSpec
        {
            public ClinicSpec(IClinic clinic, ISpeciality spec)
            {
                Clinic = clinic;
                Spec = spec;
            }

            public IClinic Clinic { get; set; }
            public ISpeciality Spec { get; set; }

            public bool Equals(ClinicSpec x)
            {
                if (x == null)
                    return false;

                return string.Equals(x.Clinic.Id, Clinic.Id) && string.Equals(x.Spec.Id, Spec.Id);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ClinicSpec);
            }

            public int GetHashCode(ClinicSpec clinicSpec)
            {
                return clinicSpec.Clinic.Id.GetHashCode() ^ clinicSpec.Spec.Id.GetHashCode();
            }

            public override int GetHashCode()
            {
                return GetHashCode(this);
            }
        }

        private readonly IDistrictCollectionDataResolver _districtCollectionDataResolver = ContainerHolder.Container.Resolve<IDistrictCollectionDataResolver>();
        private readonly IDistrictCollectionParser _districtCollectionParser = ContainerHolder.Container.Resolve<IDistrictCollectionParser>();

        private readonly IClinicCollectionDataResolver _clinicCollectionDataResolver = ContainerHolder.Container.Resolve<IClinicCollectionDataResolver>();
        private readonly IClinicCollectionParser _clinicCollectionParser = ContainerHolder.Container.Resolve<IClinicCollectionParser>();

        private readonly ISpecialityCollectionDataResolver _specialityCollectionDataResolver = ContainerHolder.Container.Resolve<ISpecialityCollectionDataResolver>();
        private readonly ISpecialityCollectionParser _specialityCollectionParser = ContainerHolder.Container.Resolve<ISpecialityCollectionParser>();

        private readonly IDoctorCollectionDataResolver _doctorCollectionDataResolver = ContainerHolder.Container.Resolve<IDoctorCollectionDataResolver>();
        private readonly IDoctorCollectionParser _doctorCollectionParser = ContainerHolder.Container.Resolve<IDoctorCollectionParser>();

        private readonly ITicketCollectionDataResolver _ticketCollectionDataResolver = ContainerHolder.Container.Resolve<ITicketCollectionDataResolver>();
        private readonly ITicketCollectionParser _ticketCollectionParser = ContainerHolder.Container.Resolve<ITicketCollectionParser>();

        private IList<IDistrict> _districts = new List<IDistrict>();
        private IDictionary<IDistrict, IList<IClinic>> _clinics = new Dictionary<IDistrict, IList<IClinic>>();
        private IDictionary<IClinic, IList<ISpeciality>> _specialities = new Dictionary<IClinic, IList<ISpeciality>>();

        public IList<IDistrict> GetDistricts()
        {
            if (!_districts.Any())
                loadDistricts();

            return _districts;
        }
        
        public IList<IClinic> GetClinics(IDistrict district)
        {
            if (!_clinics.ContainsKey(district))
                loadClinics(district);

            return _clinics[district];
        }

        public IList<ISpeciality> GetSpecialities(IClinic clinic)
        {
            if (!_specialities.ContainsKey(clinic))
                loadSpecialities(clinic);

            return _specialities[clinic];
        }

        public IList<IDoctor> GetDoctors(IClinic clinic, ISpeciality speciality)
        {
            var key = new ClinicSpec(clinic, speciality);
            return loadDoctors(key);
        }

        public IList<ITicket> GetTickets(IClinic clinic, IDoctor doctor)
        {
            return loadTickets(clinic, doctor).ToList();
        }

        private void loadDistricts()
        {
            _districts.Clear();
            string content = _districtCollectionDataResolver.RequestProcess();
            _districts = _districtCollectionParser.ParseDistricts(content);
        }

        private void loadClinics(IDistrict district)
        {
            string content = _clinicCollectionDataResolver.RequestProcess(district.Id);
            _clinics[district] = _clinicCollectionParser.ParseClinics(content);
        }

        private void loadSpecialities(IClinic clinic)
        {
            string content = _specialityCollectionDataResolver.RequestProcess(clinic.Id);
            _specialities[clinic] = _specialityCollectionParser.ParseSpecialities(content);
        }

        private IList<IDoctor> loadDoctors(ClinicSpec clinicSpec)
        {
            string content = _doctorCollectionDataResolver.RequestProcess(clinicSpec.Clinic.Id, clinicSpec.Spec.Id);
            return _doctorCollectionParser.ParseDoctors(content);
        }

        private IReadOnlyCollection<ITicket> loadTickets(IClinic clinic, IDoctor doctor)
        {
            string content = _ticketCollectionDataResolver.RequestProcess(clinic.Id, doctor.Id);
            return _ticketCollectionParser.Parse(content);
        }
    }
}
