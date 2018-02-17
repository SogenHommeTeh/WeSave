using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeSave.Data.Level2
{
    public class RentalModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        public static List<RentalModel> FromData(DataModel data)
        {
            var models = data.Rentals.Select((x, i) =>
            {
                var model = new RentalModel
                {
                    Id = i + 1,
                };

                if (x.StartDate > x.EndDate) throw new Exception("Wrong rental dates.");
                var car = data.Cars.FirstOrDefault(y => y.Id == x.CarId);
                if (car == null) throw new Exception("Car not found.");

                var days = (x.EndDate - x.StartDate).Days + 1;
                var discount = 0.0;
                if (days > 10) discount = 0.5;
                else if (days > 4) discount = 0.3;
                else if (days > 1) discount = 0.1;
                model.Price = (long)(car.PricePerDay - discount * car.PricePerDay) * days + car.PricePerKm * x.Distance;
                return model;
            }).ToList();

            return models;
        }
    }

    public class Output
    {
        [JsonProperty("rentals")]
        public List<RentalModel> Rentals { get; set; }
    }
}
