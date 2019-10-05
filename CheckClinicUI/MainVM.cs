using System;
using System.Windows.Threading;

namespace CheckClinicUI
{
    class MainVM
    {
        public ClinicVM Clinic { get; set; }
        public SpecialityVM Speciality { get; set; }
        private DispatcherTimer _timer = new DispatcherTimer();

        public MainVM()
        {
            Clinic = new ClinicVM(StaticData.ClinicId.Clinic62);
            Speciality = new SpecialityVM();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += onTimerTick;
            _timer.Start();
        }

        private void onTimerTick(object sender, EventArgs e)
        {
            Clinic.Recalc();
            Speciality.Recalc();
        }
    }
}
