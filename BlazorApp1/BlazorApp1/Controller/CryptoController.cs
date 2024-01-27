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
                var result = await client.GetAsync(endpoint);
                var json = await result.Content.ReadAsStringAsync();

                var listings = JsonSerializer.Deserialize<Listings>(json);

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
                        MarketCap = roundedMarketCap,// Converted to Billions
                        ID = coin.cmc_rank
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
        [HttpGet("trending")]
        public async Task<ActionResult<List<CryptoCoin>>> trendingAPICall()
        {
            try
            {
                var convert1 = "USD,SEK";
                var endpoint1 = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/trending/gainers-losers?convert={convert1}"); // valde listings mest för test


                // endpoint.Query = queryString.ToString();

                string API_KEY1 = "63f9b42b-7067-41cf-803e-00025ed9b664"; // min egna api key har 10000 kostnadsfria calls per månad
                var client1 = new HttpClient();
                client1.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY1);
                client1.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result1 = client1.GetAsync(endpoint1).Result;
                var json1 = result1.Content.ReadAsStringAsync().Result;

                var trendings = JsonSerializer.Deserialize<Trending>(json1);

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
        
        [HttpGet("{cryptoId:int}")]
        public async Task<ActionResult<CryptoCoin>> GetCryptoById(int? cryptoId)
        {
            try
            {
                var limit = 1;
                var convert = "USD,SEK";
                var endpoint = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert={convert}");

                string API_KEY = "63f9b42b-7067-41cf-803e-00025ed9b664";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result = await client.GetAsync(endpoint);

                if (!result.IsSuccessStatusCode)
                {
                    
                    return StatusCode((int)result.StatusCode, "Error retrieving data from the API");
                }

                var json = await result.Content.ReadAsStringAsync();
                var listings = JsonSerializer.Deserialize<Listings>(json);

                
                var selectedCoin = listings.data.FirstOrDefault(coin => coin.cmc_rank == cryptoId);

                if (selectedCoin == null)
                {
                    
                    return NotFound($"CryptoCoin with id '{cryptoId}' not found");
                }

                
                float roundedPercentChange = (float)Math.Round(selectedCoin.quote.USD.percent_change_24h, 2);
                float BmarketCap = selectedCoin.quote.USD.market_cap / 1000000000;
                float roundedMarketCap = (float)Math.Round(BmarketCap, 3);

                var cryptoCoin = new CryptoCoin
                {
                    Price = selectedCoin.quote.USD.price,
                    Summary = selectedCoin.symbol.ToUpper(),
                    CryptoName = selectedCoin.name.ToUpper(),
                    PriceSEK = selectedCoin.quote.SEK.price,
                    PercentChange = roundedPercentChange,
                    MarketCap = roundedMarketCap, // Converted to Billions
                    ID = selectedCoin.cmc_rank
                };

                return cryptoCoin;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the API");
            }
        }
        [HttpGet("{cryptoName}")]
        public async Task<ActionResult<CryptoCoin>> GetCryptoByName(string? cryptoName)
        {
            try
            {
                var limit = 1;
                var convert = "USD,SEK";
                var endpoint = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert={convert}");

                string API_KEY = "63f9b42b-7067-41cf-803e-00025ed9b664";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result = await client.GetAsync(endpoint);

                if (!result.IsSuccessStatusCode)
                {
                    
                    return StatusCode((int)result.StatusCode, "Error retrieving data from the API");
                }

                var json = await result.Content.ReadAsStringAsync();
                var listings = JsonSerializer.Deserialize<Listings>(json);

               
                var selectedCoin = listings.data.FirstOrDefault(coin => coin.name.ToLower() == cryptoName.ToLower());

                if (selectedCoin == null)
                {
                    
                    return NotFound($"CryptoCoin with name '{cryptoName}' not found");
                }

                
                float roundedPercentChange = (float)Math.Round(selectedCoin.quote.USD.percent_change_24h, 2);
                float BmarketCap = selectedCoin.quote.USD.market_cap / 1000000000;
                float roundedMarketCap = (float)Math.Round(BmarketCap, 3);

                var cryptoCoin = new CryptoCoin
                {
                    Price = selectedCoin.quote.USD.price,
                    Summary = selectedCoin.symbol.ToUpper(),
                    CryptoName = selectedCoin.name.ToUpper(),
                    PriceSEK = selectedCoin.quote.SEK.price,
                    PercentChange = roundedPercentChange,
                    MarketCap = roundedMarketCap, // Converted to Billions
                    ID = selectedCoin.cmc_rank
                };

                return cryptoCoin;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the API");
            }
        }

        [HttpGet("/{cryptoName}/news")]

        public async Task<string> GetCryptoNews(string cryptoName)
        {
            var endpoint = new Uri($"https://wdim.moralis.io/api/v1/news");

            string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJub25jZSI6IjU5OTk4OGU1LTkwYzMtNGY5Yy05YjEyLTEzNjQxNGQyOTJhOCIsIm9yZ0lkIjoiMzczNDU2IiwidXNlcklkIjoiMzgzNzk5IiwidHlwZUlkIjoiNWYzYWUxYTMtNjEyMi00ZTk1LWEyNzktZThjZWY0ZGUzZDZhIiwidHlwZSI6IlBST0pFQ1QiLCJpYXQiOjE3MDU4MjE0ODIsImV4cCI6NDg2MTU4MTQ4Mn0.dvaycdQowhAZU3_gvkC08CebxbnjwZfNh8A92JfNo7A";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");
            var result = await client.GetAsync(endpoint);
            var json = await result.Content.ReadAsStringAsync();

            return cryptoName;

        }
        [HttpGet("performance/all")]
        public async Task<ActionResult<List<CryptoCoin>>> GetPerformance()
        {
            try
            {
                var limit = 100;
                var convert1 = "USD,SEK";
                var endpoint1 = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert={convert1}&limit={limit}"); // valde listings mest för test


                // endpoint.Query = queryString.ToString();

                string API_KEY1 = "63f9b42b-7067-41cf-803e-00025ed9b664"; // min egna api key har 10000 kostnadsfria calls per månad
                var client1 = new HttpClient();
                client1.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY1);
                client1.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result1 = client1.GetAsync(endpoint1).Result;
                var json1 = result1.Content.ReadAsStringAsync().Result;
                var listings = JsonSerializer.Deserialize<Listings>(json1);
                List<string> cryptoSymbolsList = new List<string>();

                foreach (var crypto in listings.data)
                {
                    cryptoSymbolsList.Add(crypto.symbol.ToString());
                }


                string cryptoSymbolsString = string.Join(",", cryptoSymbolsList);

                var timePeriod = "all_time,yesterday,24h,7d,30d,90d,365d";
                var id = cryptoSymbolsString.ToUpper();
                var endpoint = $"https://pro-api.coinmarketcap.com/v2/cryptocurrency/price-performance-stats/latest?symbol={id}&time_period={timePeriod}";
                string apiKey = "63f9b42b-7067-41cf-803e-00025ed9b664";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");
                var result = await client.GetAsync(endpoint);
                var json = await result.Content.ReadAsStringAsync();
                var performances = JsonSerializer.Deserialize<Performances>(json);
                // market_cap: 554278688.6651919, market_cap: 563851743.1169437
                List<CryptoCoin> cryptoCoin = new();

                foreach (var coin in listings.data)
                {
                    float roundedPercentChange = (float)Math.Round(coin.quote.USD.percent_change_24h, 2);
                    float BmarketCap = coin.quote.USD.market_cap / 1000000000;
                    float roundedMarketCap = (float)Math.Round(BmarketCap, 3);
                    var symbolToSearch = coin.symbol; // replace with the symbol you're looking for
                                                      //var performanceData = performances.data.FirstOrDefault(pair => pair.Key == symbolToSearch).Value;


                    var performanceData = performances.data.Where(x => x.Key == coin.symbol).FirstOrDefault().Value.FirstOrDefault();

                    float? openPrice = performanceData.periods._30d.quote.USD.open;
                    float? percentChange30days = performanceData.periods._30d.quote.USD.percent_change;
                    float? closePrice = performanceData.periods._30d.quote.USD.close;
                    float? high = performanceData.periods._30d.quote.USD.high;
                    float? low = performanceData.periods._30d.quote.USD.low;

                    cryptoCoin.Add(new CryptoCoin
                    {
                        Price = coin.quote.USD.price,
                        Summary = coin.symbol.ToUpper(),
                        CryptoName = coin.name.ToUpper(),
                        PriceSEK = coin.quote.SEK.price,
                        PercentChange = roundedPercentChange,
                        MarketCap = roundedMarketCap,// Converted to Billions
                        ID = coin.cmc_rank,
                        OpenPrice = openPrice,
                        PercentChange30days = percentChange30days,
                        ClosePrice = closePrice,
                        Low = low,
                        High = high,

                    });
                    //   var bitcoinValueSek = Convert.ToDecimal(bitcoinValueUsd) * 10; // får fram real bitcoin data i USD valuta.
                }
                return cryptoCoin;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


    }
}
