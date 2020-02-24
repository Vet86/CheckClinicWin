using Autofac;
using CheckClinic.DataParser;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;

namespace CheckClinic.Detector
{
    public class DetectorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TicketCollectionDataResolver>().As<ITicketCollectionDataResolver>();
            builder.RegisterType<TicketCollectionParser>().As<ITicketCollectionParser>();
            builder.RegisterType<DataRequest.DataRequest>().As<IDataRequest>();
            builder.RegisterType<MailSettings.MailSettings>().As<IMailSettings>();
            builder.RegisterType<MailNotifier.MailNotifier>().As<IMailNotifier>();
            base.Load(builder);
        }
    }
}