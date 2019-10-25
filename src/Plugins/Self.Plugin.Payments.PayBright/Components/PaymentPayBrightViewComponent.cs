using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Services.Localization;
using Self.Plugin.Payments.PayBright.Models;
using Self.Plugin.Payments.PayBright.Services;

namespace Self.Plugin.Payments.PayBright.Components
{
    [ViewComponent(Name = "PaymentPayBright")]
    public class PaymentPayBrightViewComponent : ViewComponent
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly PaymentPayBrightService _paymentPayBrightService;

        #endregion

        #region Ctor

        public PaymentPayBrightViewComponent(ILocalizationService localizationService,
            IWorkContext workContext,
            PaymentPayBrightService paymentPayBrightService)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._paymentPayBrightService = paymentPayBrightService;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel();

            //prepare years
            for (var i = 0; i < 15; i++)
            {
                var year = (DateTime.Now.Year + i).ToString();
                model.ExpireYears.Add(new SelectListItem { Text = year, Value = year, });
            }

            //prepare months
            for (var i = 1; i <= 12; i++)
            {
                model.ExpireMonths.Add(new SelectListItem { Text = i.ToString("D2"), Value = i.ToString(), });
            }

            //prepare card types


            return View("~/Plugins/Payments.PayBright/Views/PaymentInfo.cshtml", model);
        }

        #endregion
    }
}