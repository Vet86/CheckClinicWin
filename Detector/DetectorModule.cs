using Autofac;
using CheckClinic.DataParser;
using CheckClinic.Model;
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
            //builder.RegisterType<ObserveData>().As<IObserveData>().WithParameter(new TypedParameter(typeof(string), "clinicId";
            base.Load(builder);
        }
    }
}
