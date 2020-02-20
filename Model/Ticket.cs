using Newtonsoft.Json;

namespace CheckClinic.Model
{
    //public class DateTicketCollection
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("date_start")]
    //    public DateTicket DateStart { get; set; }
    //}

    public class Ticket
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date_start")]
        public DateTicket DateStart { get; set; }
    }

    public class DateTicket
    {
        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("day_verbose")]
        public string DayVerbose { get; set; }

        [JsonProperty("month")]
        public string Month { get; set; }

        [JsonProperty("month_verbose")]
        public string MonthVerbose { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("iso")]
        public string Iso { get; set; }

        [JsonProperty("day")]
        public string Day { get; set; }
    }
}
