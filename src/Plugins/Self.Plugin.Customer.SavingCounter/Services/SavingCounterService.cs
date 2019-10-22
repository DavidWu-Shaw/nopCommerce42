using Nop.Services.Configuration;

namespace Self.Plugin.Customer.SavingCounter.Services
{
    public class SavingCounterService : ISavingCounterService
    {
        private readonly ISettingService _settingService;

        public SavingCounterService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public decimal GetTotalSavings(bool includeBaseAmount = true)
        {
            decimal totalSavings = _settingService.GetSettingByKey<decimal>(SavingCounterDefaults.ACTUAL_TOTAL_SAVINGS_SETTINGS_KEY);
            if (includeBaseAmount)
            {
                decimal baseAmount = _settingService.GetSettingByKey<decimal>(SavingCounterDefaults.BASE_AMOUNT_SETTINGS_KEY);
                totalSavings += baseAmount;
            }

            return totalSavings;
        }

        public void AdjustAmountBySettingKey(string key, decimal amount)
        {
            // Cumulate base amount by delta amount
            decimal originalValue = _settingService.GetSettingByKey<decimal>(key);
            _settingService.SetSetting(key, originalValue + amount);
        }
    }
}
