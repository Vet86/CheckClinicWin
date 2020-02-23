using Autofac;
using CheckClinic.Interfaces;

namespace CheckClinic.Console
{
    public class CheckClinicConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Detector.Detector>().As<IDetector>();
            base.Load(builder);
        }
    }
}
