using RestSharp;
using System.IO;
using System.Threading.Tasks;
using static CheckClinicUI.StaticData;

namespace CheckClinicDataResolver
{
    public class ClinicJsonWriter
    {
        private string _file;
        private RestClient _client = new RestClient("https://www.gorzdrav.spb.ru/api/check_clinic/");
        RestRequest _request;

        public ClinicJsonWriter(string file)
        {
            _file = file;
            _request = new RestRequest(Method.POST);
            _request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            _request.AddHeader("Host", "www.gorzdrav.spb.ru");
            _request.AddHeader("X-Requested-With", "XMLHttpRequest");
        }

        public void RequestProcess(ClinicId clinicId)
        {
            _request.AddParameter("clinic_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);

            IRestResponse response = _client.Execute(_request);
            string json = response.Content;
            File.WriteAllText(string.Format(_file, (int)clinicId), json);
        }

        public async Task RequestProcessAsync(ClinicId clinicId)
        {
            await Task.Run(() => RequestProcess(clinicId));
        }
    }
}
