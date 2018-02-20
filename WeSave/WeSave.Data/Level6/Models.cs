using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeSave.Data.Level6
{
    public class RentalOutput : Data.RentalOutput
    {
        [JsonIgnore]
        public override long Price { get; set; }

        [JsonIgnore]
        public override OptionsOutput Options { get; set; }

        [JsonIgnore]
        public override CommissionOutput Commission { get; set; }
    }

    public class Output : AOutput<DataModel>
    {
        [JsonProperty("rentals_modifications")]
        public List<RentalModificationOutput> RentalModifications { get; set; }

        public override AOutput<DataModel> FromData(DataModel data)
        {
            RentalModifications = data.RentalModifications.Select((rentalModification, i) =>
            {
                var model = new RentalModificationOutput
                {
                    Id = i + 1,
                    RentalId = rentalModification.RentalId,
                };

                var rental = data.Rentals.FirstOrDefault(rentalModel => rentalModel.Id == rentalModification.RentalId);
                if (rental == null)
                    throw new Exception("Rental not found.");
                if (rental.StartDate > rental.EndDate)
                    throw new Exception("Wrong rental dates.");
                var car = data.Cars.FirstOrDefault(carModel => carModel.Id == rental.CarId);
                if (car == null)
                    throw new Exception("Car not found.");

                var rentalOriginal = new RentalOutput()
                .ComputeDiscountPrice(rental, car)
                .ComputeOptions(rental)
                .ComputeCommission(rental)
                .ComputeActions(rental);

                if (rentalOriginal.Commission.DrivyFee < 0)
                    throw new Exception("Wrong rental, cannot extract Drivy fee.");

                if (rentalModification.StartDate.HasValue)
                    rental.StartDate = rentalModification.StartDate.Value;
                if (rentalModification.EndDate.HasValue)
                    rental.EndDate = rentalModification.EndDate.Value;
                if (rentalModification.Distance.HasValue)
                    rental.Distance = rentalModification.Distance.Value;

                var rentalModified = new RentalOutput()
                    .ComputeDiscountPrice(rental, car)
                    .ComputeOptions(rental)
                    .ComputeCommission(rental)
                    .ComputeActions(rental);

                model.ComputeActions(rentalOriginal, rentalModified);

                return model;
            }).ToList();
            return this;
        }
    }
}
