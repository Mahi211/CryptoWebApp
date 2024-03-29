﻿using BlazorApp1.Controller;

namespace BlazorApp1.Models    
{
    
    public class CryptoCoin
    {
        public string? CryptoName { get; set; }
        public float? Price { get; set; }
        public string? Summary { get; set; }
        public float? PriceSEK { get; set; }
        public float? PercentChange { get; set; }
        public float? MarketCap { get; set; }

        public float? PercentChange30days { get; set; }

        public float? OpenPrice { get; set; }
        public float? ClosePrice { get; set; }
        public float? High { get; set; }
        public float? Low { get; set; }
        
        
        public int ID { get; set; }
        // public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        
    }
}
