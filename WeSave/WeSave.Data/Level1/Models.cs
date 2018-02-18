using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeSave.Data.Level1
{
    public class RentalModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }
    }

    public class Output
    {
        [JsonProperty("rentals")]
        public List<RentalModel> Rentals { get; set; }

        public Output(DataModel data)
        {
            Rentals = data.Rentals.Select((x, i) =>
            {
                var model = new RentalModel
                {
                    Id = i + 1,
                };

                if (x.StartDate > x.EndDate) throw new Exception("Wrong rental dates.");
                var car = data.Cars.FirstOrDefault(y => y.Id == x.CarId);
                if (car == null) throw new Exception("Car not found.");

                model.Price = car.PricePerDay * ((x.EndDate - x.StartDate).Days + 1) + car.PricePerKm * x.Distance;
                return model;
            }).ToList();
        }
    }
}
