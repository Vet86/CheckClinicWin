using Autofac;
using CheckClinic.Interfaces;
using CheckClinic.Model;
using CommandLineParser.Exceptions;

namespace CheckClinic.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var parametersValid = false;
            var argsParser = new ArgsParser();
            while (!parametersValid)
            {
                parametersValid = true;
                var parser = new CommandLineParser.CommandLineParser();
                parser.ExtractArgumentAttributes(argsParser);
                try
                {
                    parser.ParseCommandLine(args);
                    parser.ShowParsedArguments();
                    parametersValid = argsParser.IsValid();
                }
                catch (CommandLineException e)
                {
                    System.Console.WriteLine(e.Message);
                    parametersValid = false;
                }
                if (!parametersValid)
                {
                    args = System.Console.ReadLine().Split(' ');
                }
            }
            
            if (argsParser.Generate)
            {
                var cacheGenerator = ContainerHolder.Container.Resolve<ICacheGenerator>();
                cacheGenerator.Process();
                return;
            }
            var detector = ContainerHolder.Container.Resolve<IDetector>();
            foreach(var receiver in argsParser.GetMailReceivers())
            {
                detector.AddMailReceiver(new System.Net.Mail.MailAddress(receiver));
            }

            var doctorNames = argsParser.GetDoctorNames();
            var doctorIds = argsParser.GetDoctorIds();
            var clinicIds = argsParser.GetClinicIds();
            for(int i = 0; i < doctorIds.Length; ++i)
            {
                detector.Add(new ObserveData(clinicIds[i], doctorIds[i], doctorNames?[i]));
            }

            System.Console.ReadKey();
        }
    }
}
