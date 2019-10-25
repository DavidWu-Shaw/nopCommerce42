using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Stores;
using Self.Plugin.Payments.PayBright.Services;

namespace Self.Plugin.Payments.PayBright
{
    /// <summary>
    /// Represents PayBright payment method
    /// </summary>
    public class PaymentPayBrightMethod : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWebHelper _webHelper;
        private readonly PaymentPayBrightService _paymentPayBrightService;
        private readonly PaymentPayBrightSettings _paymentPayBrightSettings;

        #endregion

        #region Ctor

        public PaymentPayBrightMethod(ICurrencyService currencyService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ILogger logger,
            IOrderTotalCalculationService orderTotalCalculationService,
            ISettingService settingService,
            IStoreService storeService,
            IWebHelper webHelper,
            PaymentPayBrightService paymentPayBrightService,
            PaymentPayBrightSettings paymentPayBrightSettings)
        {
            this._currencyService = currencyService;
            this._customerService = customerService;
            this._genericAttributeService = genericAttributeService;
            this._httpContextAccessor = httpContextAccessor;
            this._localizationService = localizationService;
            this._logger = logger;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._settingService = settingService;
            this._storeService = storeService;
            this._webHelper = webHelper;
            this._paymentPayBrightService = paymentPayBrightService;
            this._paymentPayBrightSettings = paymentPayBrightSettings;
        }

        #endregion



        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            string authorizationGetUrl = "https://sandbox.paybright.com/CheckOut/AppForm.aspx";
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();

            #region Hard coded test Values

            queryParameters.Add("x_account_id", "bUUBFD4FmSvkO3RTyJAmSvSBL6yOg2ZTCFXWuPDjdTuXKqanfg");
            queryParameters.Add("x_amount", "1000.00");
            queryParameters.Add("x_currency", "CAD");
            queryParameters.Add("x_customer_billing_address1", "161 Bay Street");
            queryParameters.Add("x_customer_billing_address2", "123");
            queryParameters.Add("x_customer_billing_city", "Toronto");
            queryParameters.Add("x_customer_billing_company", "PayBright");
            queryParameters.Add("x_customer_billing_country", "CA");
            queryParameters.Add("x_customer_billing_phone", "4161234567");
            queryParameters.Add("x_customer_billing_state", "ON");
            queryParameters.Add("x_customer_billing_zip", "N9B 2L1");
            queryParameters.Add("x_customer_email", "david@buywell.com");
            queryParameters.Add("x_customer_first_name", "John");
            queryParameters.Add("x_customer_last_name", "Smith");
            queryParameters.Add("x_customer_phone", "4161234567");
            queryParameters.Add("x_customer_shipping_address1", "161 Bay Street");
            queryParameters.Add("x_customer_shipping_address2", "123");
            queryParameters.Add("x_customer_shipping_city", "Toronto");
            queryParameters.Add("x_customer_shipping_company", "PayBright");
            queryParameters.Add("x_customer_shipping_country", "CA");
            queryParameters.Add("x_customer_shipping_first_name", "John");
            queryParameters.Add("x_customer_shipping_last_name", "Smith");
            queryParameters.Add("x_customer_shipping_phone", "4161234567");
            queryParameters.Add("x_customer_shipping_state", "ON");
            queryParameters.Add("x_customer_shipping_zip", "N9B 2L1");
            queryParameters.Add("x_description", "37");
            queryParameters.Add("x_invoice", "37");
            queryParameters.Add("x_locale", "EN");
            queryParameters.Add("x_platform", "custom");
            queryParameters.Add("x_reference", "37");
            queryParameters.Add("x_shop_country", "CA");
            queryParameters.Add("x_shop_name", "PayBright Demo Store");
            queryParameters.Add("x_test", "true");
            queryParameters.Add("x_url_callback", "https://www.buywell.com/");
            queryParameters.Add("x_url_cancel", "https://www.buywell.com/sale");
            queryParameters.Add("x_url_complete", "https://www.buywell.com/shop-by-concern");

            #endregion Hard coded test Values

            queryParameters.Add("x_signature", "BEFFFB36BEF03BF57842E865012663392F10E730F83DC86AF0B8A55C94CB26AC");

            authorizationGetUrl = QueryHelpers.AddQueryString(authorizationGetUrl, queryParameters);

            _httpContextAccessor.HttpContext.Response.Redirect(authorizationGetUrl);

        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return 0m;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            if (capturePaymentRequest == null)
            {
                throw new ArgumentException(nameof(capturePaymentRequest));
            }

            //set amount in USD
            //   var amount = _currencyService.ConvertCurrency(capturePaymentRequest.Order.OrderTotal, capturePaymentRequest.Order.CurrencyRate);

            //whether there are non-downloadable order items
            // var nonDownloadable = capturePaymentRequest.Order.OrderItems.Any(item => !item.Product?.IsDownload ?? true);

            //capture transaction

            //sucessfully captured
            return new CapturePaymentResult
            {
                NewPaymentStatus = PaymentStatus.Paid,
                CaptureTransactionId = "",
                CaptureTransactionResult = "",


            };
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            if (refundPaymentRequest == null)
            {
                throw new ArgumentException(nameof(refundPaymentRequest));
            }

            var cdnCurrency = _currencyService.GetCurrencyByCode("CAD");

            if (cdnCurrency == null)
            {
                throw new NopException("CAD currency cannot be loaded");
            }


            RefundPaymentResult processPR = new RefundPaymentResult();
            return processPR;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            if (voidPaymentRequest == null)
            {
                throw new ArgumentException(nameof(voidPaymentRequest));
            }

            return new VoidPaymentResult
            {
                NewPaymentStatus = PaymentStatus.Voided
            };

        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            if (processPaymentRequest == null)
            {
                throw new ArgumentException(nameof(processPaymentRequest));
            }


            //Queue up recurring payment in aws lambda... 
            //Use SQS? 
            //Save to DB this is recurring...
            return null;


        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            if (cancelPaymentRequest == null)
            {
                throw new ArgumentException(nameof(cancelPaymentRequest));
            }

            //always success
            //Send Notification to AWS Lambda this is cancelled?  or use DB Flag...
            return new CancelRecurringPaymentResult();
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            //it's not a redirection payment method. So we always return false
            return false;
        }

        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>List of validating errors</returns>
        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            if (form == null)
            {
                throw new ArgumentException(nameof(form));
            }

            //try to get errors
            if (form.TryGetValue("Errors", out StringValues errorsString) && !StringValues.IsNullOrEmpty(errorsString))
            {
                return new[] { errorsString.ToString() }.ToList();
            }

            return new List<string>();
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>Payment info holder</returns>
        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentPayBright/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying plugin in public store ("payment info" checkout step)
        /// </summary>
        /// <returns>View component name</returns>
        public string GetPublicViewComponentName()
        {
            return "PaymentPayBright";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new PaymentPayBrightSettings
            {
                UseSandbox = true
            });

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.PayBright.PaymentMethodDescription", "Pay by credit card using PayBright");

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<PaymentPayBrightSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.PayBright.PaymentMethodDescription");

            base.Uninstall();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get { return RecurringPaymentType.Manual; }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.Redirection; }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        public string PaymentMethodDescription
        {
            //return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
            //for example, for a redirection payment method, description may be like this: "You will be redirected to PayPal site to complete the payment"
            get { return _localizationService.GetResource("Plugins.Payments.PayBright.PaymentMethodDescription"); }
        }

        public void CheckAndSetSettings()
        {
            if (_paymentPayBrightSettings.UseSandbox)
            {
                _paymentPayBrightSettings.ApiKey = "bUUBFD4FmSvkO3RTyJAmSvSBL6yOg2ZTCFXWuPDjdTuXKqanfg";
                _paymentPayBrightSettings.ApiToken = "7tDSThDC6P8OgXORfkXXqsp7FaD6EDmbDwLbVCmtTCyFqOLkPr";
                _paymentPayBrightSettings.AuthorizationPostUrl = "https://sandbox.paybright.com/CheckOut/ApplicationForm.aspx";
                _paymentPayBrightSettings.StoreId = "store5";
                _paymentPayBrightSettings.DynamicDescriptor = "BXXWXXX";
                _paymentPayBrightSettings.Crypt = "7";
            }
        }
        #endregion
    }
}