using System.Text.Json.Serialization;


namespace BlazorApp1.Models
{
    public class Performances
    {
        public Status status { get; set; }
        public Dictionary<string, List<CoinPerformance>> data { get; set; }


        public class Status
        {
            public DateTime timestamp { get; set; }
            public int error_code { get; set; }
            public object error_message { get; set; }
            public int elapsed { get; set; }
            public int credit_count { get; set; }
            public object notice { get; set; }
        }

        public class Data
        {
            public List<CoinPerformance> CoinPerformance { get; set; }
        }

        public class CoinPerformance
        {
            public int id { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public string slug { get; set; }
            public DateTime last_updated { get; set; }
            public Periods periods { get; set; }
        }

        public class Periods
        {
            public All_Time all_time { get; set; }
            [JsonPropertyName("365d")]
            public All_Time _365d { get; set; }

            [JsonPropertyName("90d")]
            public All_Time _90d { get; set; }

            [JsonPropertyName("30d")]
            public All_Time _30d { get; set; }

            [JsonPropertyName("7d")]
            public All_Time _7d { get; set; }
            public All_Time yesterday { get; set; }

            [JsonPropertyName("24h")]
            public All_Time _24h { get; set; }
        }

        public class All_Time
        {
            public DateTime open_timestamp { get; set; }
            public DateTime high_timestamp { get; set; }
            public DateTime low_timestamp { get; set; }
            public DateTime close_timestamp { get; set; }
            public Quote quote { get; set; }
        }

        public class Quote
        {
            public USD USD { get; set; }
        }

        public class USD
        {
            public float? open { get; set; }
            public DateTime? open_timestamp { get; set; }
            public float? high { get; set; }
            public DateTime? high_timestamp { get; set; }
            public float? low { get; set; }
            public DateTime? low_timestamp { get; set; }
            public float? close { get; set; }
            public DateTime? close_timestamp { get; set; }
            public float? percent_change { get; set; }
            public float? price_change { get; set; }
        }



    }

}
