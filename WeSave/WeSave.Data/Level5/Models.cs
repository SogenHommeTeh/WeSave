using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeSave.Data.Level5
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
                    Options = new OptionsModel(),
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
                if (x.DeductibleReduction == true)
                {
                    model.Options.DeductibleReduction = days * 400;
                }
                model.Actions = new List<ActionModel>
                {
                    new ActionModel
                    {
                        Who = ActionActor.Driver,
                        Amount = -(model.Price + model.Options.DeductibleReduction),
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Owner,
                        Amount = model.Price - fee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Insurance,
                        Amount = model.Commission.InsuranceFee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Assistance,
                        Amount = model.Commission.AssistanceFee,
                    },
                    new ActionModel
                    {
                        Who = ActionActor.Drivy,
                        Amount = model.Commission.DrivyFee + model.Options.DeductibleReduction,
                    },
                };
                return model;
            }).ToList();
        }
    }
}
