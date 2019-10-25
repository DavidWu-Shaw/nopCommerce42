using Self.Plugin.Payments.PayBright.Models;
using Self.Plugin.Payments.PayBright.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System;
using Nop.Services.Messages;

namespace Self.Plugin.Payments.PayBright.Controllers
{
    public class PaymentPayBrightController : BasePaymentController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly PaymentPayBrightService _paymentPayBrightService;
        private readonly PaymentPayBrightSettings _paymentPayBrightSettings;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        #endregion

        #region Ctor

        public PaymentPayBrightController(ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IWorkContext workContext,
            PaymentPayBrightService paymentPayBrightService,
            PaymentPayBrightSettings paymentPayBrightSettings,
            IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            IStoreContext storeContext,
            IStoreService storeService)
        {
            this._customerService = customerService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._notificationService = notificationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._workContext = workContext;
            this._paymentPayBrightService = paymentPayBrightService;
            this._paymentPayBrightSettings = paymentPayBrightSettings;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._storeContext = storeContext;
            this._storeService = storeService;
        }

        #endregion

        #region Methods
        [Area(AreaNames.Admin)]
        [AuthorizeAdmin]
        public IActionResult Configure()
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var payBrightPaymentSettings = _settingService.LoadSetting<PaymentPayBrightSettings>(storeScope);

            //prepare model
            var model = new ConfigurationModel
            {
                StoreId = payBrightPaymentSettings.StoreId,
                ApiToken = payBrightPaymentSettings.ApiToken,
                UseSandbox = payBrightPaymentSettings.UseSandbox,
                ValidateAddress = payBrightPaymentSettings.ValidateAddress,
                AdditionalFee = payBrightPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = payBrightPaymentSettings.AdditionalFeePercentage,
                AwsKey = payBrightPaymentSettings.AwsKey,
                AwsSecretKey = payBrightPaymentSettings.AwsSecretKey,
                Crypt = payBrightPaymentSettings.Crypt,
                DynamicDescriptor = payBrightPaymentSettings.DynamicDescriptor,
                PublicKey = payBrightPaymentSettings.PublicKey,
                ActiveStoreScopeConfiguration = storeScope

            };
            if (storeScope > 0)
            {
                model.StoreId_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.StoreId, storeScope);
                model.ApiToken_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.ApiToken, storeScope);
                model.UseSandbox_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.UseSandbox, storeScope);
                model.ValidateAddress_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.ValidateAddress, storeScope);
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.AdditionalFeePercentage, storeScope);
                model.AwsKey_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.AwsAuthenticationKey, storeScope);
                model.AwsSecretKey_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.AwsSecretKey, storeScope);
                model.Crypt_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.Crypt, storeScope);
                model.DynamicDescriptor_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.DynamicDescriptor, storeScope);
                model.PublicKey_OverrideForStore = _settingService.SettingExists(payBrightPaymentSettings, x => x.PublicKey, storeScope);
            }
            return View("~/Plugins/Payments.PayBright/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [Area(AreaNames.Admin)]
        [AuthorizeAdmin]
        public IActionResult Configure(ConfigurationModel model)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var payBrightPaymentSettings = _settingService.LoadSetting<PaymentPayBrightSettings>(storeScope);

            //save settings
            payBrightPaymentSettings.StoreId = model.StoreId;
            payBrightPaymentSettings.ApiToken = model.ApiToken;
            payBrightPaymentSettings.UseSandbox = model.UseSandbox;
            payBrightPaymentSettings.ValidateAddress = model.ValidateAddress;
            payBrightPaymentSettings.AdditionalFee = model.AdditionalFee;
            payBrightPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            payBrightPaymentSettings.AwsKey = model.AwsKey;
            payBrightPaymentSettings.AwsSecretKey = model.AwsSecretKey;
            payBrightPaymentSettings.DynamicDescriptor = model.DynamicDescriptor;
            payBrightPaymentSettings.Crypt = model.Crypt;
            payBrightPaymentSettings.PublicKey = model.PublicKey;

            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.StoreId, model.StoreId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.ApiToken, model.ApiToken_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.ValidateAddress, model.ValidateAddress_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.AwsKey, model.AwsKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.AwsSecretKey, model.AwsSecretKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.DynamicDescriptor, model.DynamicDescriptor_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.Crypt, model.Crypt_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(payBrightPaymentSettings, x => x.PublicKey, model.PublicKey_OverrideForStore, storeScope, false);

            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult CompletePaymentHandler(AuthorizationResponseModel model)
        {
            // Hard coded orderId, should be taken from response
            // TODO: take parameters from payment response
            int orderId = 14563;
            string gateway_reference = "31293";
            string gateway_message = "Success";

            var order = _orderService.GetOrderById(orderId);
            if (order != null)
            {
                //order note
                order.OrderNotes.Add(new OrderNote
                {
                    Note = "PayBright payment process: " + gateway_message,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);
            }

                //valid
            if (_orderProcessingService.CanMarkOrderAsPaid(order))
            {
                order.AuthorizationTransactionId = gateway_reference;
                _orderService.UpdateOrder(order);

                _orderProcessingService.MarkOrderAsPaid(order);
            }

            // redirect to checkout complete page
            return RedirectToRoute("CheckoutCompleted", new { orderId = orderId });
        }

        public IActionResult CancelPaymentHandler()
        {
            return RedirectToRoute("CheckoutOnePage");
        }

        #endregion
    }
}
