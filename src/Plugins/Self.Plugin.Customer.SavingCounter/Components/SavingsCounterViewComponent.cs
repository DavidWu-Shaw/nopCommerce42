using Self.Plugin.Customer.SavingCounter.Models;
using Self.Plugin.Customer.SavingCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Localization;

namespace Self.Plugin.Customer.SavingCounter.Components
{
    [ViewComponent(Name = "SavingsCounter")]
    public class SavingsCounterViewComponent : ViewComponent
    {
        #region Fields

        private readonly ISavingCounterService _savingCounterService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly SavingCounterSettings _savingCounterSettings;

        #endregion

        #region Ctor

        public SavingsCounterViewComponent(ILocalizationService localizationService,
            IWorkContext workContext,
            IPriceFormatter priceFormatter,
            SavingCounterSettings savingCounterSettings,
            ISavingCounterService savingCounterService)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._savingCounterService = savingCounterService;
            this._priceFormatter = priceFormatter;
            this._savingCounterSettings = savingCounterSettings;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            if (this.Request.Cookies.ContainsKey("disableCounter") && this.Request.Cookies["disableCounter"] == "1")
            {
                return Content("");
            }
            string totalSavings = _priceFormatter.FormatPrice(_savingCounterService.GetTotalSavings(true));
            int minutesOfCounterRefresh = _savingCounterSettings.MinutesOfCounterRefresh == 0 ? 1 : _savingCounterSettings.MinutesOfCounterRefresh;
            SavingCounterInfoModel model = new SavingCounterInfoModel
            {
                TotalSavings = totalSavings,
                MillisecondsOfCounterRefresh = minutesOfCounterRefresh * 60 * 1000
            };

            return View("~/Plugins/Customer.SavingCounter/Views/SavingsCounterInfo.cshtml", model);
        }

        #endregion
    }
}