using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeSave.Data.Level6
{
    public class ActionModel
    {
        [JsonProperty("who")]
        public ActionActor Who { get; set; }

        [JsonProperty("type")]
        public ActionType Type => _amount < 0 ? ActionType.Debit : ActionType.Credit;

        private long _amount;

        [JsonProperty("amount")]
        public long Amount
        {
            get => _amount < 0 ? -_amount : _amount;
            set => _amount = value;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        [EnumMember(Value = "debit")]
        Debit,
        [EnumMember(Value = "credit")]
        Credit,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionActor
    {
        [EnumMember(Value = "driver")]
        Driver,
        [EnumMember(Value = "owner")]
        Owner,
        [EnumMember(Value = "insurance")]
        Insurance,
        [EnumMember(Value = "assistance")]
        Assistance,
        [EnumMember(Value = "drivy")]
        Drivy,
    }

    public class OptionsModel
    {
        [JsonProperty("deductible_reduction")]
        public long DeductibleReduction { get; set; }
    }
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

        [JsonIgnore]
        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonIgnore]
        [JsonProperty("options")]
        public OptionsModel Options { get; set; }

        [JsonIgnore]
        [JsonProperty("commission")]
        public CommissionModel Commission { get; set; }

        [JsonProperty("actions")]
        public List<ActionModel> Actions { get; set; }

        public RentalModel(DataModel data, Data.RentalModel rental)
        {
            Options = new OptionsModel();
            Commission = new CommissionModel();
            if (rental.StartDate > rental.EndDate) throw new Exception("Wrong rental dates.");
            var car = data.Cars.FirstOrDefault(y => y.Id == rental.CarId);
            if (car == null) throw new Exception("Car not found.");

            var days = (rental.EndDate - rental.StartDate).Days + 1;
            var discount = 0.0;
            if (days > 10) discount = 0.5;
            else if (days > 4) discount = 0.3;
            else if (days > 1) discount = 0.1;
            Price = (long)(car.PricePerDay - discount * car.PricePerDay) * days + car.PricePerKm * rental.Distance;
            var fee = (long)(Price * 0.3);
            Commission.InsuranceFee = fee / 2;
            Commission.AssistanceFee = days * 100;
            Commission.DrivyFee = fee - (Commission.InsuranceFee + Commission.AssistanceFee);
            if (Commission.DrivyFee < 0) throw new Exception("Wrong rental, cannot extract Drivy fee.");
            if (rental.DeductibleReduction == true)
            {
                Options.DeductibleReduction = days * 400;
            }
            Actions = new List<ActionModel>
                {
                    new ActionModel
                    {
                        Who = ActionActor.Driver,
                        Amount = -(Price + Options.DeductibleReduction),
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Owner,
                        Amount = Price - fee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Insurance,
                        Amount = Commission.InsuranceFee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Assistance,
                        Amount = Commission.AssistanceFee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Drivy,
                        Amount = Commission.DrivyFee + Options.DeductibleReduction,
                    },
                };
        }
    }


    public class RentalModificationModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rental_id")]
        public int RentalId { get; set; }

        [JsonProperty("actions")]
        public List<ActionModel> Actions { get; set; }
    }

    public class Output
    {
        [JsonProperty("rentals_modifications")]
        public List<RentalModificationModel> RentalModifications { get; set; }

        public Output(DataModel data)
        {
            RentalModifications = data.RentalModifications.Select((x, i) =>
            {
                var model = new RentalModificationModel
                {
                    Id = i + 1,
                    RentalId = x.RentalId,
                    Actions = new List<ActionModel>()
                };
                var rentalIn = data.Rentals.FirstOrDefault(y => y.Id == x.RentalId);
                if (rentalIn == null) throw new Exception("Rental not found.");
                var rental = new RentalModel(data, rentalIn);
                if (x.StartDate.HasValue) rentalIn.StartDate = x.StartDate.Value;
                if (x.EndDate.HasValue) rentalIn.EndDate = x.EndDate.Value;
                if (x.Distance.HasValue) rentalIn.Distance = x.Distance.Value;
                var rentalModified = new RentalModel(data, rentalIn);

                model.Actions = rental.Actions.Select(y =>
                {
                    var action = rentalModified.Actions.First(z => z.Who == y.Who);
                    var amount = y.Amount;
                    if (y.Type == ActionType.Debit) amount = -amount;
                    var amountModified = action.Amount;
                    if (action.Type == ActionType.Debit) amountModified = -amountModified;
                    return new ActionModel
                    {
                        Who = y.Who,
                        Amount = amountModified - amount,
                    };
                }).ToList();

                return model;
            }).ToList();
        }
    }
}
