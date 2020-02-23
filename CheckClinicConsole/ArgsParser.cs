using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;

namespace CheckClinic.Console
{
    public class ArgsParser
    {
        private string[] _doctorIds;
        private string[] _doctorNames;
        private string[] _clinicIds;
        private string[] _mailReceivers;

        [ValueArgument(typeof(string), 'd', "doctor_id", Description = "Doctor ids. Separate with ;",Optional = false)]
        public string DoctorId;

        [ValueArgument(typeof(string), 'n', "doctor_name", Description = "Doctor names. Separate with ;", Optional = true)]
        public string DoctorName;

        [ValueArgument(typeof(string), 'c', "clinic_id", Description = "Clinic ids. Separate with ;", Optional = false)]
        public string ClinicId;

        [ValueArgument(typeof(string), 'm', "mail", Description = "Mail receivers. Separate with ;", Optional = false)]
        public string MailReceiver;

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(DoctorId) || 
                string.IsNullOrWhiteSpace(ClinicId) || 
                string.IsNullOrWhiteSpace(MailReceiver))
            {
                return false;
            }
            _clinicIds = ClinicId.Split(';');
            _doctorIds = DoctorId.Split(';');
            _mailReceivers = MailReceiver.Split(';');
            _doctorNames = DoctorName?.Split(';');

            if (_clinicIds.Length != _doctorIds.Length)
                throw new CommandLineException("clinic_id must have count like doctor_id");

            if (_doctorNames != null && _doctorNames.Length != _doctorIds.Length)
                throw new CommandLineException("doctor_name must be empty or must have count like doctor_id");

            foreach (var mail in _mailReceivers)
            {
                if (!isValidEmail(mail))
                    throw new CommandLineException($"mail {mail} is not valid");
            }
            return true;
        }

        public string[] GetDoctorIds() => _doctorIds;
        public string[] GetDoctorNames() => _doctorNames;
        public string[] GetClinicIds() => _clinicIds;
        public string[] GetMailReceivers() => _mailReceivers;

        private static bool isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
