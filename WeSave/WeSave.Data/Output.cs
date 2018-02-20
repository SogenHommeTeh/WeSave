using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeSave.Data
{
    public class ActionOutput
    {
        [JsonProperty("who")]
        public virtual ActionActor Who { get; set; }

        [JsonIgnore]
        public virtual long Difference { get; private set; }

        [JsonProperty("type")]
        public virtual ActionType Type => Difference < 0 ? ActionType.Debit : ActionType.Credit;

        [JsonProperty("amount")]
        public virtual long Amount
        {
            get => Difference < 0 ? -Difference : Difference;
            set => Difference = value;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        [EnumMember(Value = "debit")] Debit,
        [EnumMember(Value = "credit")] Credit,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionActor
    {
        [EnumMember(Value = "driver")] Driver,
        [EnumMember(Value = "owner")] Owner,
        [EnumMember(Value = "insurance")] Insurance,
        [EnumMember(Value = "assistance")] Assistance,
        [EnumMember(Value = "drivy")] Drivy,
    }

    public class OptionsOutput
    {
        [JsonProperty("deductible_reduction")]
        public virtual long DeductibleReduction { get; set; }
    }

    public class CommissionOutput
    {
        [JsonProperty("insurance_fee")]
        public virtual long InsuranceFee { get; set; }

        [JsonProperty("assistance_fee")]
        public virtual long AssistanceFee { get; set; }

        [JsonProperty("drivy_fee")]
        public virtual long DrivyFee { get; set; }
    }

    public class RentalOutput
    {
        [JsonProperty("id")]
        public virtual int Id { get; set; }

        [JsonProperty("price")]
        public virtual long Price { get; set; }

        [JsonProperty("options")]
        public virtual OptionsOutput Options { get; set; }

        [JsonProperty("commission")]
        public virtual CommissionOutput Commission { get; set; }

        [JsonProperty("actions")]
        public virtual List<ActionOutput> Actions { get; set; }

        public RentalOutput ComputeBasicPrice(RentalModel rental, CarModel car)
        {
            Price = 0;
            var days = (rental.EndDate - rental.StartDate).Days + 1;
            Price += car.PricePerDay * days + car.PricePerKm * rental.Distance;
            return this;
        }

        public RentalOutput ComputeDiscountPrice(RentalModel rental, CarModel car)
        {
            Price = 0;
            var days = (rental.EndDate - rental.StartDate).Days + 1;
            if (days > 10)
            {
                Price += (long)(car.PricePerDay - 0.5 * car.PricePerDay) * (days - 10);
                Price += (long)(car.PricePerDay - 0.3 * car.PricePerDay) * 6;
                Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * 3;
            }
            else if (days > 4)
            {
                Price += (long)(car.PricePerDay - 0.3 * car.PricePerDay) * (days - 4);
                Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * 3;
            }
            else if (days > 1)
            {
                Price += (long)(car.PricePerDay - 0.1 * car.PricePerDay) * (days - 1);
            }
            Price += car.PricePerDay + car.PricePerKm * rental.Distance;
            return this;
        }

        public RentalOutput ComputeOptions(RentalModel rental)
        {
            Options = new OptionsOutput();
            var days = (rental.EndDate - rental.StartDate).Days + 1;
            if (rental.DeductibleReduction == true)
            {
                Options.DeductibleReduction = days * 400;
            }
            return this;
        }

        public RentalOutput ComputeCommission(RentalModel rental)
        {
            Commission = new CommissionOutput();
            var days = (rental.EndDate - rental.StartDate).Days + 1;
            var fee = (long)(Price * 0.3);
            Commission.InsuranceFee = fee / 2;
            Commission.AssistanceFee = days * 100;
            Commission.DrivyFee = fee - (Commission.InsuranceFee + Commission.AssistanceFee);
            return this;
        }

        public RentalOutput ComputeActions(RentalModel rental)
        {
            Actions = new List<ActionOutput>();
            var fee = (long)(Price * 0.3);
            Actions.Add(new ActionOutput
            {
                Who = ActionActor.Driver,
                Amount = -(Price + Options.DeductibleReduction),
            });
            Actions.Add(new ActionOutput
            {
                Who = ActionActor.Owner,
                Amount = Price - fee,
            });
            Actions.Add(new ActionOutput
            {
                Who = ActionActor.Insurance,
                Amount = Commission.InsuranceFee,
            });
            Actions.Add(new ActionOutput
            {
                Who = ActionActor.Assistance,
                Amount = Commission.AssistanceFee,
            });
            Actions.Add(new ActionOutput
            {
                Who = ActionActor.Drivy,
                Amount = Commission.DrivyFee + Options.DeductibleReduction,
            });
            return this;
        }
    }

    public class RentalModificationOutput
    {
        [JsonProperty("id")]
        public virtual int Id { get; set; }

        [JsonProperty("rental_id")]
        public virtual int RentalId { get; set; }

        [JsonProperty("actions")]
        public virtual List<ActionOutput> Actions { get; set; }

        public RentalModificationOutput ComputeActions(RentalOutput original, RentalOutput modified)
        {
            Actions = new List<ActionOutput>();
            original.Actions.ForEach(originalAction =>
            {
                var modifiedAction = modified.Actions.First(action => action.Who == originalAction.Who);

                Actions.Add(new ActionOutput
                {
                    Who = originalAction.Who,
                    Amount = modifiedAction.Difference - originalAction.Difference,
                });
            });
            return this;
        }
    }
}