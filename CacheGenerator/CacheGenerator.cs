using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

        private List<Task> _specTasks = new List<Task>();
        private List<Task> _clinicTasks = new List<Task>();
        private List<Task> _districtsTasks = new List<Task>();
        class SpecInfo
        {
            public string SpecialitiesFolder;
            public string ClinicId;
            public string SpecialityId;
        }
        private List<SpecInfo> _specs = new List<SpecInfo>();

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
            var folder = "Cache";
            if (Directory.Exists("Cache"))
                Directory.Delete("Cache", true);

            if (!Directory.Exists("Cache"))
                Directory.CreateDirectory("Cache");

            _specTasks.Clear();
            _clinicTasks.Clear();
            _districtsTasks.Clear();
            _specs.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string content = _districtCollectionDataResolver.RequestProcess();
            File.WriteAllText($"{folder}/Districts.html", content);
            var districts = _districtCollectionParser.ParseDistricts(content);
            var districtsFolder = $"{folder}/Districts";
            if (!Directory.Exists(districtsFolder))
                Directory.CreateDirectory(districtsFolder);
            int i = 0;
            foreach (IDistrict district in districts)
            {
                i++;
                System.Console.WriteLine($"_________District {district.Name} {i}/{districts.Count} _________");
                //_districtsTasks.Add( parseDistrict(districtsFolder, district.Id) );
                parseDistrict(districtsFolder, district.Id).GetAwaiter().GetResult();

                if (i == 2)
                    break;
            }

            foreach(var s in _specs)
            {
                _specTasks.Add(parseSpeciality(s.SpecialitiesFolder, s.ClinicId, s.SpecialityId));
            }

            //Parallel.ForEach(_specs, (s) =>
            //{
            //    parseSpeciality(s.SpecialitiesFolder, s.ClinicId, s.SpecialityId);
            //});

            //Task.WhenAll(_districtsTasks).GetAwaiter().GetResult();
            //Task.WhenAll(_clinicTasks).GetAwaiter().GetResult();
            Task.WhenAll(_specTasks).GetAwaiter().GetResult();

            stopwatch.Stop();
            Console.WriteLine("Потрачено тактов на выполнение: " + stopwatch.ElapsedTicks);
        }

        public async Task parseDistrict(string districtsFolder, string districtId)
        {
            //if (districtId != "12")
            //return;
            //await Task.Run(() =>
            {
                string content = _clinicCollectionDataResolver.RequestProcess(districtId);

                string districtFolder = $"{districtsFolder}/{districtId}";
                File.WriteAllText($"{districtFolder}.json", content);

                string clinicsFolder = $"{districtFolder}/Clinics";
                if (!Directory.Exists(clinicsFolder))
                    Directory.CreateDirectory(clinicsFolder);

                var clinics = _clinicCollectionParser.ParseClinics(content);
                int i = 0;
                foreach (IClinic clinic in clinics)
                {
                    i++;
                    System.Console.WriteLine($"Clinic {i}/{clinics.Count}");
                    //_clinicTasks.Add(parseClinic(clinicsFolder, clinic.Id));
                    parseClinic(clinicsFolder, clinic.Id).GetAwaiter().GetResult();
                    if (i == 10)
                        break;
                }
            }//);
        }

        public async Task parseClinic(string clinicsFolder, string clinicId)
        {
            //if (clinicId != "255")
            //return;

            //await Task.Run(() =>
            {
                string content = _specialityCollectionDataResolver.RequestProcess(clinicId);

                string clinicFilder = $"{clinicsFolder}/{clinicId}";
                string clinicFileName = $"{clinicFilder}.json";
                if (File.Exists(clinicFileName))
                {
                    System.Diagnostics.Trace.WriteLine($"{clinicFileName} is exists");
                    return;
                }
                File.WriteAllText(clinicFileName, content);

                string specialitiesFolder = $"{clinicFilder}/Specialities";
                if (!Directory.Exists(specialitiesFolder))
                    Directory.CreateDirectory(specialitiesFolder);

                var specialities = _specialityCollectionParser.ParseSpecialities(content);
                //List<Task> tasks = new List<Task>();
                int i = 0;

                //_specTasks.Clear();
                foreach (ISpeciality speciality in specialities)
                {
                    i++;
                    _specs.Add(new SpecInfo()
                    {
                        SpecialitiesFolder = specialitiesFolder,
                        ClinicId = clinicId,
                        SpecialityId = speciality.Id
                    });
                    //_specTasks.Add(parseSpeciality(specialitiesFolder, clinicId, speciality.Id));
                    //parseSpeciality(specialitiesFolder, clinicId, speciality).GetAwaiter().GetResult();
                    if (i == 5)
                        break;
                }
                //Task.WhenAll(_specTasks).GetAwaiter().GetResult();
            }//);
        }

        public async Task parseSpeciality(string specialitiesFolder, string clinicId, string specialityId)
        {
            writeLog(specialityId, "1");
            string content = string.Empty;
            //await Task.Run(() =>
            {
                writeLog(specialityId, "2");
                content = _doctorCollectionDataResolver.RequestProcess(clinicId, specialityId);
                writeLog(specialityId, "3");

                writeLog(specialityId, "4");
                string correctSpecId = specialityId.Replace("/", "").Replace("\\", "");

                string specialityFolder = $"{specialitiesFolder}/{correctSpecId}_{clinicId}";
                if (!Directory.Exists(specialityFolder))
                    Directory.CreateDirectory(specialityFolder);


                string specialityFileName = $"{specialityFolder}.json";
                if (File.Exists(specialityFileName))
                {
                    System.Diagnostics.Trace.WriteLine($"{specialityFileName} is exists");
                    return;
                }
                File.WriteAllText(specialityFileName, content);
                writeLog(specialityId, "5");

                //if (!Directory.Exists("Cache/Doctors"))
                //    Directory.CreateDirectory("Cache/Doctors");

                //var doctors = _doctorCollectionParser.ParseDoctors(content);
                //foreach (IDoctor doctor in doctors)
                //{
                //    parseDoctor(doctor);
                //}

            }//);
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

        private void writeLog(string tag1, string tag2)
        {
            var text = $"{DateTime.Now.TimeOfDay.ToString()} {tag1} {tag2}. Thread {Thread.CurrentThread.ManagedThreadId}" + "\n";
            System.Console.WriteLine(text);
            //File.AppendAllText("l.txt", text);
        }
    }
}
