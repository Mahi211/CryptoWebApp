using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BlazorApp1.Components.Pages.Weather;

namespace BlazorApp1.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<CryptoCoin>> GetCrypto(int test)
        {
            var limit = 1;
            var convert = "USD";
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
                cryptoCoin.Add(new CryptoCoin
                {
                    Price = coin.quote.USD.price,
                    Summary = coin.symbol.ToUpper(),
                    CryptoName = coin.name.ToUpper()
                });
                //   var bitcoinValueSek = Convert.ToDecimal(bitcoinValueUsd) * 10; // får fram real bitcoin data i USD valuta.
            }

            return cryptoCoin;
        }
    }
}
