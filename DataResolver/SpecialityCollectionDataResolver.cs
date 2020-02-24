using CheckClinic.Interfaces;
using RestSharp;

namespace CheckClinicDataResolver
{
    public class SpecialityCollectionDataResolver : ISpecialityCollectionDataResolver
    {
        public string RequestProcess(string clinicId)
        {
            RestClient client = new RestClient("https://www.gorzdrav.spb.ru/api/check_clinic/");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("clinic_form-clinic_id", clinicId, ParameterType.GetOrPost);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
