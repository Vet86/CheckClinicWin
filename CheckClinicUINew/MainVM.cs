using Autofac;
using CheckClinic.Interfaces;
using CheckClinic.Model;
using CheckClinicUI.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CheckClinic.UI
{
    class MainVM : ViewModelBase
    {
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

        private IDistrict _selectDistrict;
        private IClinic _selectClinic;
        private ISpeciality _selectSpeciality;
        private IDoctor _selectDoctor;
        private ITicket _selectTicket;

        public MainVM()
        {
            string content = _districtCollectionDataResolver.RequestProcess();
            Districts = _districtCollectionParser.ParseDistricts(content);
        }

        public IList<IDistrict> Districts { get; set; }
        public IDistrict SelectDistrict
        {
            get
            {
                return _selectDistrict;
            }
            set
            {
                if (_selectDistrict == value)
                    return;

                _selectDistrict = value;
                FirePropertyChange(nameof(SelectDistrict));

                string content = _clinicCollectionDataResolver.RequestProcess(_selectDistrict.Id);
                Clinics = _clinicCollectionParser.ParseClinics(content);
                FirePropertyChange(nameof(Clinics));
            }
        }

        public IList<IClinic> Clinics { get; set; }
        public IClinic SelectClinic
        {
            get
            {
                return _selectClinic;
            }
            set
            {
                if (_selectClinic == value)
                    return;

                _selectClinic = value;
                FirePropertyChange(nameof(SelectClinic));

                if (_selectClinic != null)
                {
                    string content = _specialityCollectionDataResolver.RequestProcess(_selectClinic.Id);
                    Specialities = _specialityCollectionParser.ParseSpecialities(content);
                }
                else
                {
                    Specialities = null;
                }
                FirePropertyChange(nameof(Specialities));
            }
        }

        public IList<ISpeciality> Specialities { get; set; }
        public ISpeciality SelectSpeciality
        {
            get
            {
                return _selectSpeciality;
            }
            set
            {
                if (_selectSpeciality == value)
                    return;

                _selectSpeciality = value;
                FirePropertyChange(nameof(SelectSpeciality));

                if (_selectSpeciality != null)
                {
                    string content = _doctorCollectionDataResolver.RequestProcess(_selectClinic.Id, _selectSpeciality.Id);
                    Doctors = _doctorCollectionParser.ParseDoctors(content);
                }
                else
                {
                    Doctors = null;
                }
                FirePropertyChange(nameof(Doctors));
            }
        }

        public IList<IDoctor> Doctors { get; set; }
        public IDoctor SelectDoctor
        {
            get
            {
                return _selectDoctor;
            }
            set
            {
                if (_selectDoctor == value)
                    return;

                _selectDoctor = value;
                FirePropertyChange(nameof(SelectDoctor));

                if (_selectDoctor != null)
                {
                    string content = _ticketCollectionDataResolver.RequestProcess(_selectClinic.Id, _selectDoctor.Id);
                    Tickets = _ticketCollectionParser.Parse(content);
                }
                else
                {
                    Tickets = null;
                }
                FirePropertyChange(nameof(Tickets));
            }
        }

        public IReadOnlyCollection<ITicket> Tickets { get; set; }
        public ITicket SelectTicket
        {
            get
            {
                return _selectTicket;
            }
            set
            {
                if (_selectTicket == value)
                    return;

                _selectTicket = value;
                FirePropertyChange(nameof(SelectTicket));
            }
        }

        public ObservableCollection<IObserveData> ObserveData { get; } = new ObservableCollection<IObserveData>();

        internal void AddOservable(IDoctor doctor)
        {
            var observeData = new ObserveData(_selectClinic.Id, doctor.Id, doctor.DoctorName);
            ObserveData.Add(observeData);
        }
    }
}
