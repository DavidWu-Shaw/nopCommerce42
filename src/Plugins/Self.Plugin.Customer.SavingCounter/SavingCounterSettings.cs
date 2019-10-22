using Nop.Core.Configuration;

namespace Self.Plugin.Customer.SavingCounter
{
    public class SavingCounterSettings : ISettings
    {
        public decimal InitialBaseAmount { get; set; }
        public decimal DeltaAmountMax { get; set; }
        public int MinutesOfCounterRefresh { get; set; }
    }
}