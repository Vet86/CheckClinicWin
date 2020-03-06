using Autofac;
using CheckClinic.Interfaces;
using CheckClinic.Model;
using CheckClinicUI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CheckClinic.UI
{
    class MainVM : ViewModelBase, INotifyPropertyChanged, IDetectListener
    {
        #region private fields 
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

        private readonly IDetector _detector = ContainerHolder.Container.Resolve<IDetector>();
        #endregion

        public MainVM()
        {
            _detector.AddListener(this);
            RemoveObserverCommand = new RelayCommand(removeObserver, x => true);
            OpenSettingsCommand = new RelayCommand(x => openSettings(), y => true);
            string content = _districtCollectionDataResolver.RequestProcess();
            Districts = _districtCollectionParser.ParseDistricts(content);
            IsDistrictsExpanded = true;
            refreshObserves();

            PropertyChanged += onPropertyChanged;
        }

        private void onPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectDistrict):
                    processDistrictSelection();
                    break;

                case nameof(SelectClinic):
                    processClinicSelection();
                    break;

                case nameof(SelectSpeciality):
                    processSpecialitySelection();
                    break;

                case nameof(SelectDoctor):
                    processDoctorSelection();
                    break;

                case nameof(IsDistrictsExpanded):
                    processIsDistrictsExpanded();
                    break;

                case nameof(IsClinicsExpanded):
                    processIsClinicsExpanded();
                    break;

                case nameof(IsSpecialitiesExpanded):
                    processIsSpecialitiesExpanded();
                    break;

                case nameof(IsDoctorsExpanded):
                    processIsDoctorsExpanded();
                    break;
            }
        }

        private void processIsDoctorsExpanded()
        {
            if (!IsDoctorsExpanded)
                return;

            IsDistrictsExpanded = false;
            IsClinicsExpanded = false;
            IsSpecialitiesExpanded = false;
            IsTicketsExpanded = false;
        }

        private void processIsSpecialitiesExpanded()
        {
            if (!IsSpecialitiesExpanded)
                return;

            IsDistrictsExpanded = false;
            IsClinicsExpanded = false;
            IsDoctorsExpanded = false;
            IsTicketsExpanded = false;
        }

        private void processIsDistrictsExpanded()
        {
            if (!IsDistrictsExpanded)
                return;

            IsClinicsExpanded = false;
            IsSpecialitiesExpanded = false;
            IsDoctorsExpanded = false;
            IsTicketsExpanded = false;
        }

        private void processIsClinicsExpanded()
        {
            if (!IsClinicsExpanded)
                return;

            IsDistrictsExpanded = false;
            IsSpecialitiesExpanded = false;
            IsDoctorsExpanded = false;
            IsTicketsExpanded = false;
        }

        private void processDoctorSelection()
        {
            if (SelectDoctor == null)
            {
                Tickets = null;
                return;
            }

            string content = _ticketCollectionDataResolver.RequestProcess(SelectClinic.Id, SelectDoctor.Id);
            Tickets = _ticketCollectionParser.Parse(content);
            IsTicketsExpanded = true;
        }

        private void processSpecialitySelection()
        {
            if (SelectSpeciality == null)
            {
                Doctors = null;
                return;
            }

            string content = _doctorCollectionDataResolver.RequestProcess(SelectClinic.Id, SelectSpeciality.Id);
            Doctors = _doctorCollectionParser.ParseDoctors(content);
            IsDoctorsExpanded = true;
        }

        private void processClinicSelection()
        {
            if (SelectClinic == null)
            {
                Specialities = null;
                return;
            }

            string content = _specialityCollectionDataResolver.RequestProcess(SelectClinic.Id);
            Specialities = _specialityCollectionParser.ParseSpecialities(content);
            IsSpecialitiesExpanded = true;
        }

        private void processDistrictSelection()
        {
            string content = _clinicCollectionDataResolver.RequestProcess(SelectDistrict.Id);
            Clinics = _clinicCollectionParser.ParseClinics(content);
            IsClinicsExpanded = true;
        }

        public IList<IDistrict> Districts { get; set; }
        public IDistrict SelectDistrict { get; set; }

        public IList<IClinic> Clinics { get; set; }
        public IClinic SelectClinic { get; set; }

        public IList<ISpeciality> Specialities { get; set; }
        public ISpeciality SelectSpeciality { get; set; }

        public IList<IDoctor> Doctors { get; set; }
        public IDoctor SelectDoctor { get; set; }

        public IReadOnlyCollection<ITicket> Tickets { get; set; }
        public ITicket SelectTicket { get; set; }

        public IReadOnlyList<ObserverDataVM> ObserveData { get; private set; }

        public bool IsDistrictsExpanded { get; set; }

        public bool IsClinicsExpanded { get; set; }

        public bool IsSpecialitiesExpanded { get; set; }

        public bool IsDoctorsExpanded { get; set; }
        public bool IsTicketsExpanded { get; set; }

        public RelayCommand RemoveObserverCommand { get; private set; }
        public RelayCommand OpenSettingsCommand { get; private set; }

        public void NewTicketsAdded(IObserveData observeData, IEnumerable<ITicket> newTickets)
        {
            var observerVM = ObserveData.FirstOrDefault(x => x.ObserveData == observeData);
            if (observerVM == null)
                return;

            observerVM.Tickets = GetTickets(observerVM.ObserveData.ClinicId, observerVM.ObserveData.DoctorId);
        }

        internal void AddOservable(IDoctor doctor)
        {
            var observeData = new ObserveData(SelectClinic.Id, doctor.Id, doctor.DoctorName);
            _detector.Add(observeData);
            refreshObserves();
        }

        private void removeObserver(object obj)
        {
            var obs = (ObserverDataVM)obj;
            _detector.Remove(obs.ObserveData);
            refreshObserves();
        }

        private void refreshObserves()
        {
            ObserveData = _detector.GetObserves().Select(x => new ObserverDataVM() { ObserveData = x, Tickets = GetTickets(x.ClinicId, x.DoctorId) }).ToList();
        }

        private IReadOnlyList<ITicket> GetTickets(string clinicId, string doctorId)
        {
            string content = _ticketCollectionDataResolver.RequestProcess(clinicId, doctorId);
            return _ticketCollectionParser.Parse(content);
        }

        private void openSettings()
        {
            var settingsView = new SettingsView();
            var settingsVM = new SettingsVM();
            foreach (var receiver in _detector.GetMailReceivers())
            {
                settingsVM.Receivers.Add(new Receiver() { Name = receiver.Address });
            }
            settingsView.DataContext = settingsVM;
            if (settingsView.ShowDialog() == true)
            {
                _detector.ClearReceivers();
                foreach (var receiver in settingsVM.Receivers)
                {
                    try
                    {
                        _detector.AddMailReceiver(new System.Net.Mail.MailAddress(receiver.Name));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}