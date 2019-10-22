using Self.Plugin.Customer.SavingCounter.Models;
using Self.Plugin.Customer.SavingCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Self.Plugin.Customer.SavingCounter.Controllers
{
    public class SavingCounterController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly IStoreContext _storeContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ISavingCounterService _savingCounterService;

        #endregion

        #region Ctor

        public SavingCounterController( 
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            INotificationService notificationService,
            IPriceFormatter priceFormatter,
            ISavingCounterService savingCounterService,
            IStoreContext storeContext)
        {

            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._notificationService = notificationService;
            this._priceFormatter = priceFormatter;
            this._storeContext = storeContext;
            this._savingCounterService = savingCounterService;
        }

        #endregion

        #region Methods
        [Area(AreaNames.Admin)]
        [AuthorizeAdmin]
        public IActionResult Configure()
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this._storeContext.ActiveStoreScopeConfiguration;
            var savingCounterSettings = _settingService.LoadSetting<SavingCounterSettings>(storeScope);

            var baseAmount = _settingService.GetSettingByKey<decimal>(SavingCounterDefaults.BASE_AMOUNT_SETTINGS_KEY);
            var actualTotalSavings = _settingService.GetSettingByKey<decimal>(SavingCounterDefaults.ACTUAL_TOTAL_SAVINGS_SETTINGS_KEY);
            //prepare model
            var model = new ConfigurationModel
            {
                InitialBaseAmount = savingCounterSettings.InitialBaseAmount,
                DeltaAmountMax = savingCounterSettings.DeltaAmountMax,
                MinutesOfCounterRefresh = savingCounterSettings.MinutesOfCounterRefresh,
                BaseAmount = _priceFormatter.FormatPrice(baseAmount),
                ActualTotalSavings = _priceFormatter.FormatPrice(actualTotalSavings),
            };

            return View("~/Plugins/Customer.SavingCounter/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [Area(AreaNames.Admin)]
        [AuthorizeAdmin]
        public IActionResult Configure(ConfigurationModel model)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this._storeContext.ActiveStoreScopeConfiguration;
            var savingCounterSettings = _settingService.LoadSetting<SavingCounterSettings>(storeScope);

            //save settings
            savingCounterSettings.InitialBaseAmount = model.InitialBaseAmount;
            savingCounterSettings.DeltaAmountMax = model.DeltaAmountMax;
            savingCounterSettings.MinutesOfCounterRefresh = model.MinutesOfCounterRefresh;

            decimal baseAmount = _settingService.GetSettingByKey<decimal>(SavingCounterDefaults.BASE_AMOUNT_SETTINGS_KEY);
            // Update baseAmount only when it's not set yet
            if (baseAmount == 0m)
            {
                _settingService.SetSetting(SavingCounterDefaults.BASE_AMOUNT_SETTINGS_KEY, model.InitialBaseAmount);
            }

            _settingService.SaveSetting(savingCounterSettings);
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult GetTotalSavings()
        {
            string totalSavings = _priceFormatter.FormatPrice(_savingCounterService.GetTotalSavings(true));

            return Content(totalSavings);
        }
        #endregion
    }
}