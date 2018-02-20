using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeSave.Data.Level4
{
    public class RentalOutput : Data.RentalOutput
    {
        [JsonIgnore]
        public override List<ActionOutput> Actions { get; set; }
    }

    public class Output : AOutput<DataModel>
    {
        [JsonProperty("rentals")]
        public List<RentalOutput> Rentals { get; set; }

        public override AOutput<DataModel> FromData(DataModel data)
        {
            Rentals = data.Rentals.Select((rental, i) =>
            {
                var model = new RentalOutput
                {
                    Id = i + 1,
                };

                if (rental.StartDate > rental.EndDate)
                    throw new Exception("Wrong rental dates.");
                var car = data.Cars.FirstOrDefault(carModel => carModel.Id == rental.CarId);
                if (car == null)
                    throw new Exception("Car not found.");

                model.ComputeDiscountPrice(rental, car)
                .ComputeOptions(rental)
                .ComputeCommission(rental);

                return model;
            }).ToList();
            return this;
        }
    }
}
