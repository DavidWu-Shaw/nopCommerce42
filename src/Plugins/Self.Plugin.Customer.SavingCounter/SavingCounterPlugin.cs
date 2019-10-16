using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Tasks;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Plugins;
using Nop.Services.Tasks;

namespace Self.Plugin.Customer.SavingCounter
{
    public class SavingCounterPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;


        #region constructor
        public SavingCounterPlugin(
            ILocalizationService localizationService,
            IScheduleTaskService scheduleTaskService,
            IWebHelper webHelper,
            ILogger logger)
        {
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _webHelper = webHelper;
            _logger = logger;
        }
        #endregion

        #region methods
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/SavingCounter/Configure";
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "SavingsCounter";
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { "header_before" };
        }

        public bool HideInWidgetList
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            // Register schedule task
            _scheduleTaskService.InsertTask(new ScheduleTask()
            {
                Enabled = true,
                Name = "Saving Counter",
                Seconds = 1800,
                StopOnError = false,
                Type = "Self.Plugin.Customer.SavingCounter.SavingCounterTask",
            });

            // Register resources
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.MinutesOfCounterRefresh", "Counter Refresh Minutes");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.TotalSavings", "Actual Total Savings");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.BaseAmount", "Base Amount");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.InitialBaseAmount", "Initial Base Amount");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.DeltaAmountMax", "Delta Amount Max");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            // De-register schedule task
            ScheduleTask task = _scheduleTaskService.GetTaskByType("Self.Plugin.Customer.SavingCounter.SavingCounterTask");
            if (task != null)
            {
                _scheduleTaskService.DeleteTask(task);
            }

            _localizationService.DeletePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.MinutesOfCounterRefresh");
            _localizationService.DeletePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.TotalSavings");
            _localizationService.DeletePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.BaseAmount");
            _localizationService.DeletePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.InitialBaseAmount");
            _localizationService.DeletePluginLocaleResource("Plugin.Customer.SavingCounter.Fields.DeltaAmountMax");

            base.Uninstall();
        }

        #endregion
    }
}
