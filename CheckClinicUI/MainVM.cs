using CheckClinicDataResolver;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CheckClinicUI
{
    class MainVM
    {
        public ClinicVM Clinic { get; set; }
        public SpecialityVM Speciality { get; set; }
        private DispatcherTimer _timer = new DispatcherTimer();
        private const string ClinicJsonFile = "Data/clinic{0}.json";
        private const string SpecialityJsonFile = "Data/speciality{0}{1}.json";
        private ClinicJsonWriter _clinicJsonWriter = new ClinicJsonWriter(ClinicJsonFile);
        private SpecialityJsonWriter _specialityJsonWriter = new SpecialityJsonWriter(SpecialityJsonFile);
        private StaticData.ClinicId _clinic = StaticData.ClinicId.Clinic62;
        private TicketIncreaseNotifier _ticketIncreaseNotifier;
        private MailNotifier _mailNotifier = new MailNotifier();

        public MainVM()
        {
            if (Directory.Exists("Data"))
                Directory.Delete("Data", true);

            Directory.CreateDirectory("Data");

            Clinic = new ClinicVM(ClinicJsonFile, _clinic);
            Speciality = new SpecialityVM(SpecialityJsonFile);
            _ticketIncreaseNotifier = new TicketIncreaseNotifier(Speciality);
            _ticketIncreaseNotifier.TicketIncreaseHandler += onTicketIncrease;
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += onTimerTick;
            CheckMailSenderCommand = new RelayCommand(x => checkMail(), y => true);
            _timer.Start();
            onTimerAsync();
        }

        private void checkMail()
        {
            _mailNotifier.SendTestMailAsync();
        }

        public bool UpdateDataFromServer { get; set; } = true;

        public ICommand CheckMailSenderCommand { get; set; }

        public string Mail
        {
            get
            {
                return _mailNotifier.MailReceiver;
            }
            set
            {
                if (_mailNotifier.MailReceiver == value)
                    return;

                _mailNotifier.MailReceiver = value;
            }
        }

        private void onTicketIncrease(ResponseDoctorModel responseDoctor)
        {
            _mailNotifier.SendEmailAsync(responseDoctor);
        }

        internal void SetSpeciality(int speciality)
        {
            Speciality.Init(SpecialityJsonFile, _clinic, speciality);
            onTimerAsync();
        }

        private void onTimerTick(object sender, EventArgs e)
        {
            onTimerAsync();
        }

        private async Task onTimerAsync()
        {
            if (UpdateDataFromServer)
            {
                await _clinicJsonWriter.RequestProcessAsync(_clinic);
                foreach (var specModel in Speciality.NotifySpecialities.ToArray())
                {
                    await _specialityJsonWriter.RequestProcessAsync(_clinic, specModel);
                }
            }

            Clinic.Recalc();
            Speciality.Recalc();
        }
    }
}
