using System;
using Newtonsoft.Json;

namespace WeSave.Data
{
    public class CarModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price_per_day")]
        public long PricePerDay { get; set; }

        [JsonProperty("price_per_km")]
        public long PricePerKm { get; set; }
    }

    public class RentalModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("car_id")]
        public int CarId { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }
    }

    public class DataModel
    {
        [JsonProperty("cars")]
        public CarModel[] Cars { get; set; }

        [JsonProperty("rentals")]
        public RentalModel[] Rentals { get; set; }
    }
}
