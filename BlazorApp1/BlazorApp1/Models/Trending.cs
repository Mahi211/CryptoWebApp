namespace BlazorApp1.Models
{
    public class Trending : Listings
    {
        public Status status { get; set; }
        public Datum[] data { get; set; }
    }
}
