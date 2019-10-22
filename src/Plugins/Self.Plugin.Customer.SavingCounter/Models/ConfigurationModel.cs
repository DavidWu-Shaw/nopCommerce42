using Nop.Web.Framework.Mvc.ModelBinding;

namespace Self.Plugin.Customer.SavingCounter.Models
{
    /// <summary>
    /// Represents the moneris configuration model
    /// </summary>
    public class ConfigurationModel
    {
        [NopResourceDisplayName("Plugin.Customer.SavingCounter.Fields.TotalSavings")]
        public string ActualTotalSavings { get; set; }

        [NopResourceDisplayName("Plugin.Customer.SavingCounter.Fields.BaseAmount")]
        public string BaseAmount { get; set; }

        [NopResourceDisplayName("Plugin.Customer.SavingCounter.Fields.InitialBaseAmount")]
        public decimal InitialBaseAmount { get; set; }

        [NopResourceDisplayName("Plugin.Customer.SavingCounter.Fields.DeltaAmountMax")]
        public decimal DeltaAmountMax { get; set; }

        [NopResourceDisplayName("Plugin.Customer.SavingCounter.Fields.MinutesOfCounterRefresh")]
        public int MinutesOfCounterRefresh { get; set; }
    }
}