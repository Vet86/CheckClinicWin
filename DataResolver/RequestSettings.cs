﻿using CheckClinic.Interfaces;

namespace CheckClinic.DataResolver
{
    public class RequestSettings : IRequestSettings
    {
        public string Site => "https://www.gorzdrav.spb.ru/signup/free/?";
    }
}
