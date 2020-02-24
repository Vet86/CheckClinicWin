using Autofac;
using CheckClinic.DataParser;
using CheckClinic.DataResolver;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;

namespace CheckClinic.Console
{
    public class CheckClinicConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Detector.Detector>().As<IDetector>();
            builder.RegisterType<RequestSettings>().As<IRequestSettings>();
            builder.RegisterType<DistrictCollectionDataResolver>().As<IDistrictCollectionDataResolver>();
            builder.RegisterType<DistrictCollectionParser>().As<IDistrictCollectionParser>();
            builder.RegisterType<ClinicCollectionDataResolver>().As<IClinicCollectionDataResolver>();
            builder.RegisterType<ClinicCollectionParser>().As<IClinicCollectionParser>();
            builder.RegisterType<SpecialityCollectionDataResolver>().As<ISpecialityCollectionDataResolver>();
            builder.RegisterType<SpecialityCollectionParser>().As<ISpecialityCollectionParser>();
            builder.RegisterType<DoctorCollectionDataResolver>().As<IDoctorCollectionDataResolver>();
            builder.RegisterType<DoctorCollectionParser>().As<IDoctorCollectionParser>();
            builder.RegisterType<CacheGenerator.CacheGenerator>().As<ICacheGenerator>();
            base.Load(builder);
        }
    }
}
