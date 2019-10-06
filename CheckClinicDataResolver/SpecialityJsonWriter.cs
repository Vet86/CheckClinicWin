using RestSharp;
using System.IO;
using System.Threading.Tasks;
using static CheckClinicUI.StaticData;

namespace CheckClinicDataResolver
{
    public class SpecialityJsonWriter
    {
        private string _file;
        private RestClient _client = new RestClient("https://www.gorzdrav.spb.ru/api/doctor_list/");
        RestRequest _request;

        public SpecialityJsonWriter(string file)
        {
            _file = file;
            _request = new RestRequest(Method.POST);
            _request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            _request.AddHeader("Host", "www.gorzdrav.spb.ru");
            _request.AddHeader("X-Requested-With", "XMLHttpRequest");
        }

        public void RequestProcess(ClinicId clinicId, int specialitiId)
        {
            _request.AddParameter("speciality_form-speciality_id", (specialitiId).ToString(), ParameterType.GetOrPost);
            _request.AddParameter("speciality_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);

            IRestResponse response = _client.Execute(_request);
            string json = response.Content;
            File.WriteAllText(string.Format(_file, (int)clinicId, specialitiId), json);
        }

        public async Task RequestProcessAsync(ClinicId clinicId, int specialitiId)
        {
            await Task.Run(() => RequestProcess(clinicId, specialitiId));
        }
    }
}
