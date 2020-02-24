using CheckClinic.Interfaces;
using System.IO;

namespace CheckClinic.CacheGenerator
{
    public class CacheGenerator : ICacheGenerator
    {
        private readonly IDistrictCollectionDataResolver _districtCollectionDataResolver;
        private readonly IDistrictCollectionParser _districtCollectionParser;

        private readonly IClinicCollectionDataResolver _clinicCollectionDataResolver;
        private readonly IClinicCollectionParser _clinicCollectionParser;

        private readonly ISpecialityCollectionDataResolver _specialityCollectionDataResolver;
        private readonly ISpecialityCollectionParser _specialityCollectionParser;

        private readonly IDoctorCollectionDataResolver _doctorCollectionDataResolver;
        private readonly IDoctorCollectionParser _doctorCollectionParser;

        public CacheGenerator(
            IDistrictCollectionDataResolver districtCollectionDataResolver, 
            IDistrictCollectionParser districtCollectionParser,
            IClinicCollectionDataResolver clinicCollectionDataResolver, 
            IClinicCollectionParser clinicCollectionParser,
            ISpecialityCollectionDataResolver specialityCollectionDataResolver, 
            ISpecialityCollectionParser specialityCollectionParser,
            IDoctorCollectionDataResolver doctorCollectionDataResolver, 
            IDoctorCollectionParser doctorCollectionParser
            )
        {
            _districtCollectionDataResolver = districtCollectionDataResolver;
            _districtCollectionParser = districtCollectionParser;

            _clinicCollectionDataResolver = clinicCollectionDataResolver;
            _clinicCollectionParser = clinicCollectionParser;

            _specialityCollectionDataResolver = specialityCollectionDataResolver;
            _specialityCollectionParser = specialityCollectionParser;

            _doctorCollectionDataResolver = doctorCollectionDataResolver;
            _doctorCollectionParser = doctorCollectionParser;
        }

        public void Process()
        {
            if (Directory.Exists("Cache"))
                Directory.Delete("Cache", true);

            if (!Directory.Exists("Cache"))
                Directory.CreateDirectory("Cache");

            string content = _districtCollectionDataResolver.RequestProcess();
            File.WriteAllText("Cache/Districts.html", content);
            var districts = _districtCollectionParser.ParseDistricts(content);
            if (!Directory.Exists("Cache/Districts"))
                Directory.CreateDirectory("Cache/Districts");
            foreach (IDistrict district in districts)
            {
                parseDistrict(district);
            }
        }

        public void parseDistrict(IDistrict district)
        {
            string content = _clinicCollectionDataResolver.RequestProcess(district.Id);
            File.WriteAllText($"Cache/Districts/{district.Id}.json", content);

            if (!Directory.Exists("Cache/Clinics"))
                Directory.CreateDirectory("Cache/Clinics");

            var clinics = _clinicCollectionParser.ParseClinics(content);
            foreach (IClinic clinic in clinics)
            {
                parseClinic(clinic);                
            }
        }

        public void parseClinic(IClinic clinic)
        {
            string content = _specialityCollectionDataResolver.RequestProcess(clinic.Id);
            string clinicFileName = $"Cache/Clinics/{clinic.Id}.json";
            if (File.Exists(clinicFileName))
            {
                System.Diagnostics.Trace.WriteLine($"{clinicFileName} is exists");
                return;
            }
            File.WriteAllText(clinicFileName, content);

            if (!Directory.Exists("Cache/Specialities"))
                Directory.CreateDirectory("Cache/Specialities");

            var specialities = _specialityCollectionParser.ParseSpecialities(content);
            foreach (ISpeciality speciality in specialities)
            {
                parseSpeciality(clinic.Id, speciality);
            }
        }

        public void parseSpeciality(string clinicId, ISpeciality speciality)
        {
            string content = _doctorCollectionDataResolver.RequestProcess(clinicId, speciality.Id);
            string specialityFileName = $"Cache/Specialities/{clinicId}_{speciality.Id.Replace("/", "").Replace("\\", "")}.json";
            if (File.Exists(specialityFileName))
            {
                System.Diagnostics.Trace.WriteLine($"{specialityFileName} is exists");
                return;
            }
            File.WriteAllText(specialityFileName, content);

            //if (!Directory.Exists("Cache/Doctors"))
            //    Directory.CreateDirectory("Cache/Doctors");

            //var doctors = _doctorCollectionParser.ParseDoctors(content);
            //foreach (IDoctor doctor in doctors)
            //{
            //    parseDoctor(doctor);
            //}
        }

        public void parseDoctor(IDoctor doctor)
        {
            //string content = _doctorCollectionDataResolver.RequestProcess(clinicId, speciality.Id);
            //string specialityFileName = $"Cache/Specialities/{clinicId}_{speciality.Id}.json";
            //if (File.Exists(specialityFileName))
            //{
            //    System.Diagnostics.Trace.WriteLine($"{specialityFileName} is exists");
            //    return;
            //}
            //File.WriteAllText(specialityFileName, content);
        }
    }
}
