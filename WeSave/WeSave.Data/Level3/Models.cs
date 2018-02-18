using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeSave.Data.Level3
{
    public class CommissionModel
    {
        [JsonProperty("insurance_fee")]
        public long InsuranceFee { get; set; }

        [JsonProperty("assistance_fee")]
        public long AssistanceFee { get; set; }

        [JsonProperty("drivy_fee")]
        public long DrivyFee { get; set; }
    }

    public class RentalModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("commission")]
        public CommissionModel Commission { get; set; }
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
                    Commission = new CommissionModel(),
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
                var fee = (long)(model.Price * 0.3);
                model.Commission.InsuranceFee = fee / 2;
                model.Commission.AssistanceFee = days * 100;
                model.Commission.DrivyFee = fee - (model.Commission.InsuranceFee + model.Commission.AssistanceFee);
                if (model.Commission.DrivyFee < 0) throw new Exception("Wrong rental, cannot extract Drivy fee.");
                return model;
            }).ToList();
        }
    }
}
