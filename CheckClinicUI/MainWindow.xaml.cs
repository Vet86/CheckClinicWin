using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace CheckClinicUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //testrequest();
            foo2();
            foo3();
        }

        public async void testrequest()
        {
            var client = new HttpClient();

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
    new KeyValuePair<string, string>("Referer", "https://www.gorzdrav.spb.ru/signup/free/?"),
});

            var stringContent = new StringContent("a");
            // Get the response.
            HttpResponseMessage response = await client.PostAsync(
                "https://www.gorzdrav.spb.ru/api/check_clinic/",
                requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                Console.WriteLine(await reader.ReadToEndAsync());
            }
        }

        public async void foo()
        {
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });

            var message = new HttpRequestMessage(HttpMethod.Post, new Uri("https://www.gorzdrav.spb.ru/api/check_clinic/"));
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            message.Headers.Referrer = new Uri("https://www.gorzdrav.spb.ru/signup/free/?");
            message.Headers.Host = "www.gorzdrav.spb.ru";
            message.Headers.Add("X-Requested-With", "XMLHttpRequest");
            

            var parameters = new Dictionary<string, string> { { "clinic_id", "255" } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            //HttpCompletionOption httpCompletionOption = new HttpCompletionOption();
            message.Properties.Add("clinic_id", "255");
            message.Properties.Add("clinic_form-clinic_id", "255");
            // message.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue())

            string content = "clinic_form-clinic_id=255";
            //message.Content = HttpContent;
            var responseMessage = await client.SendAsync(message);
            //MessageBox.Show(string.Format("Status Code: {0}{1}Content-Type: {2}{1}Date: {3}{1}Location:{4}", responseMessage.StatusCode, Environment.NewLine, responseMessage.Content.Headers.ContentType, responseMessage.Headers.Date, responseMessage.Headers.Location));
            // Get the response content.
            HttpContent responseContent = responseMessage.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                Console.WriteLine(await reader.ReadToEndAsync());
            }
        }

        public async void foo2()
        {
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });



            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("clinic_form-clinic_id", "255"),
            });

            var responseMessage = await client.PostAsync("https://www.gorzdrav.spb.ru/api/check_clinic/", formContent);
            //MessageBox.Show(string.Format("Status Code: {0}{1}Content-Type: {2}{1}Date: {3}{1}Location:{4}", responseMessage.StatusCode, Environment.NewLine, responseMessage.Content.Headers.ContentType, responseMessage.Headers.Date, responseMessage.Headers.Location));
            // Get the response content.
            HttpContent responseContent = responseMessage.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                Console.WriteLine(await reader.ReadToEndAsync());
            }
        }

        private void foo3()
        {
            var client = new RestClient("https://www.gorzdrav.spb.ru/api/check_clinic/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("clinic_form-clinic_id", "255", ParameterType.GetOrPost);
            IRestResponse response = client.Execute(request);
        }
    }
}
