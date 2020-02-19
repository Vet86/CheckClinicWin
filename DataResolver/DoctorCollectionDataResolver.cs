using RestSharp;

namespace CheckClinicDataResolver
{
    public class DoctorCollectionDataResolver
    {
        public string RequestProcess(string clinicId, string specialitiId)
        {
            RestClient client = new RestClient("https://www.gorzdrav.spb.ru/api/doctor_list/");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("speciality_form-speciality_id", specialitiId, ParameterType.GetOrPost);
            request.AddParameter("speciality_form-clinic_id", clinicId, ParameterType.GetOrPost);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
