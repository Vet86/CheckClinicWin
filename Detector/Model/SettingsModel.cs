using System;
using System.Collections.Generic;

namespace CheckClinic.Detector.Model
{
    public class SettingsModel
    {
        public Version Version;
        public List<ReceiverModel> ReceiverModels { get; set; } = new List<ReceiverModel>();
        public TimeSpan Interval { get; set; }
    }
}
