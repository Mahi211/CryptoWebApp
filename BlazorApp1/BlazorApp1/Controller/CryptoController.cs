using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace BlazorApp1.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<CryptoCoin>>> GetCryptos()
        {
            try
            {
                var limit = 1;
                var convert = "USD,SEK";
                var endpoint = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert={convert}"); // valde listings mest för test


                // endpoint.Query = queryString.ToString();

                string API_KEY = "63f9b42b-7067-41cf-803e-00025ed9b664"; // min egna api key har 10000 kostnadsfria calls per månad
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;

                var listings = System.Text.Json.JsonSerializer.Deserialize<Listings>(json);

                List<CryptoCoin> cryptoCoin = new();

                foreach (var coin in listings.data)
                {
                    float roundedPercentChange = (float)Math.Round(coin.quote.USD.percent_change_24h, 2);
                    float BmarketCap = coin.quote.USD.market_cap / 1000000000;
                    float roundedMarketCap = (float)Math.Round(BmarketCap, 3);
                    cryptoCoin.Add(new CryptoCoin
                    {
                        Price = coin.quote.USD.price,
                        Summary = coin.symbol.ToUpper(),
                        CryptoName = coin.name.ToUpper(),
                        PriceSEK = coin.quote.SEK.price,
                        PercentChange = roundedPercentChange,
                        MarketCap = roundedMarketCap // Converted to Billions
                    });
                    //   var bitcoinValueSek = Convert.ToDecimal(bitcoinValueUsd) * 10; // får fram real bitcoin data i USD valuta.
                }

                return cryptoCoin;
            }
            catch (Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the API");
            }


        }
        [HttpGet("trending/{limit1}")]
        public async Task<ActionResult<List<CryptoCoin>>> trendingAPICall(int limit1)
        {
            try
            {
                var convert1 = "USD,SEK";
                var endpoint1 = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/trending/gainers-losers?convert={convert1}&limit={limit1}"); // valde listings mest för test


                // endpoint.Query = queryString.ToString();

                string API_KEY1 = "63f9b42b-7067-41cf-803e-00025ed9b664"; // min egna api key har 10000 kostnadsfria calls per månad
                var client1 = new HttpClient();
                client1.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY1);
                client1.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result1 = client1.GetAsync(endpoint1).Result;
                var json1 = result1.Content.ReadAsStringAsync().Result;

                var trendings = System.Text.Json.JsonSerializer.Deserialize<Trending>(json1);

                List<CryptoCoin> trendingCoin = new();

                foreach (var coin in trendings.data)
                {
                    float roundedPrice = (float)Math.Round(coin.quote.SEK.price, 6);
                    float roundedPercentChange = (float)Math.Round(coin.quote.USD.percent_change_24h, 5);

                    trendingCoin.Add(new CryptoCoin
                    {
                        Price = roundedPrice,
                        Summary = coin.symbol.ToUpper(),
                        CryptoName = coin.name.ToUpper(),
                        PercentChange = roundedPercentChange
                    });


                }
                // var sortedTrendingCoin = trendingCoin.OrderByDescending(coin => coin.PercentChange).ToList();

                return trendingCoin;
            }
            
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the API");
            }
        }
    }
}
