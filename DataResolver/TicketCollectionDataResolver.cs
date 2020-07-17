using CheckClinic.Interfaces;
using RestSharp;

namespace CheckClinicDataResolver
{
    public class TicketCollectionDataResolver : ITicketCollectionDataResolver
    {
        public string RequestProcess(string clinicId, string doctorId)
        {
            RestClient client = new RestClient("https://gorzdrav.spb.ru/api/appointment_list/");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://beta.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "beta.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("doctor_form-clinic_id", clinicId, ParameterType.GetOrPost);
            request.AddParameter("doctor_form-doctor_id", doctorId, ParameterType.GetOrPost);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
