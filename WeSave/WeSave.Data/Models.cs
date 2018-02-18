using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeSave.Data
{
    public class CarModel
    {
        [JsonRequired]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonRequired]
        [JsonProperty("price_per_day")]
        public long PricePerDay { get; set; }

        [JsonRequired]
        [JsonProperty("price_per_km")]
        public long PricePerKm { get; set; }
    }

    public class RentalModel
    {
        [JsonRequired]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonRequired]
        [JsonProperty("car_id")]
        public int CarId { get; set; }

        [JsonRequired]
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonRequired]
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonRequired]
        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("deductible_reduction")]
        public bool DeductibleReduction { get; set; }
    }

    public class RentalModificationModel
    {
        [JsonRequired]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonRequired]
        [JsonProperty("rental_id")]
        public int RentalId { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("distance")]
        public long? Distance { get; set; }
    }

    public class DataModel
    {
        [JsonRequired]
        [JsonProperty("cars")]
        public List<CarModel> Cars { get; set; }

        [JsonRequired]
        [JsonProperty("rentals")]
        public List<RentalModel> Rentals { get; set; }

        [JsonProperty("rental_modifications")]
        public List<RentalModificationModel> RentalModifications { get; set; }
    }
}
