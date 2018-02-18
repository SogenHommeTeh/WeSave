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

                var days = (x.EndDate - x.StartDate).Days + 1;
                model.Price = 0;
                if (days > 10)
                {
                    model.Price += (long)(car.PricePerDay - 0.5 * car.PricePerDay) * (days - 10);
                    model.Price += (long)(car.PricePerDay - 0.3 * car.PricePerDay) * 6;
                    model.Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * 3;
                }
                else if (days > 4)
                {
                    model.Price += (long)(car.PricePerDay - 0.3 * car.PricePerDay) * (days - 4);
                    model.Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * 3;
                }
                else if (days > 1)
                {
                    model.Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * (days - 1);
                }
                model.Price += car.PricePerDay + car.PricePerKm * x.Distance;
                return model;
            }).ToList();
        }
    }
}
