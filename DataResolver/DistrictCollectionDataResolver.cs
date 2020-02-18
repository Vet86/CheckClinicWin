﻿using CheckClinic.DataResolver;
using RestSharp;

namespace CheckClinicDataResolver
{
    public class DistrictCollectionDataResolver
    {
        public string RequestProcess(IRequestSettings requestSettings)
        {
            RestClient client = new RestClient(requestSettings.Site);
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
