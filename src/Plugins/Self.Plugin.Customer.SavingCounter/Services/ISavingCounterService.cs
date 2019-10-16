namespace Self.Plugin.Customer.SavingCounter.Services
{
    public interface ISavingCounterService
    {
        decimal GetTotalSavings(bool includeBaseAmount = true);
        void AdjustAmountBySettingKey(string key, decimal amount);
    }
}
