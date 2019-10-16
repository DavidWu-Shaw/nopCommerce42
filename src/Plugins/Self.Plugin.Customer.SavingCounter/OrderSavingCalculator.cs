using Self.Plugin.Customer.SavingCounter.Services;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;
using Nop.Services.Logging;

namespace Self.Plugin.Customer.SavingCounter
{
    public class OrderSavingCalculator : IConsumer<OrderPaidEvent>
    {
        private readonly ISavingCounterService _savingCounterService;
        private readonly ILogger _logger;

        public OrderSavingCalculator(ISavingCounterService savingCounterService, ILogger logger)
        {
            _savingCounterService = savingCounterService;
            _logger = logger;
        }

        public void HandleEvent(OrderPaidEvent eventMessage)
        {
            if (eventMessage.Order != null)
            {
                if (eventMessage.Order.OrderDiscount > 0)
                {
                    _savingCounterService.AdjustAmountBySettingKey(SavingCounterDefaults.ACTUAL_TOTAL_SAVINGS_SETTINGS_KEY, eventMessage.Order.OrderDiscount);
                }
            }
            else
            {
                _logger.Error("OrderSavingCalculator - Order is null...");
            }
        }
    }
}
