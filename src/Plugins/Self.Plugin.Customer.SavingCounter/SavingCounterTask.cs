using Self.Plugin.Customer.SavingCounter.Services;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using System;

namespace Self.Plugin.Customer.SavingCounter
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class SavingCounterTask : IScheduleTask
    {
        private readonly SavingCounterSettings _settings;
        private readonly ISavingCounterService _savingCounterService;
        private readonly ILogger _logger;

        public SavingCounterTask(
            ILogger logger,
            ISavingCounterService savingCounterService,
            SavingCounterSettings settings)
        {
            _logger = logger;
            _savingCounterService = savingCounterService;
            _settings = settings;
        }

        public void Execute()
        {
            try
            {
                Random random = new Random(DateTime.Now.Minute);
                decimal deltaAmount = new decimal(random.NextDouble()) * _settings.DeltaAmountMax;

                // Cumulate base amount by delta amount
                _savingCounterService.AdjustAmountBySettingKey(SavingCounterDefaults.BASE_AMOUNT_SETTINGS_KEY, deltaAmount);

                _logger.Information(string.Format("Saving Counter Task completed with deltaAmount = {0}.", deltaAmount));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}