using Nop.Services.Logging;
using Nop.Services.Payments;
using RestSharp;

namespace Self.Plugin.Payments.PayBright.Services
{
    public class PaymentPayBrightService
    {
        private readonly ILogger _logger;
        private readonly PaymentPayBrightSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="settings"></param>
        public PaymentPayBrightService(ILogger logger, PaymentPayBrightSettings settings)
        {
            _logger = logger;
            _settings = settings;
            // TODO: Hard code some value for now, refactor required 
            _settings.ApiKey = "bUUBFD4FmSvkO3RTyJAmSvSBL6yOg2ZTCFXWuPDjdTuXKqanfg";
            _settings.ApiToken = "7tDSThDC6P8OgXORfkXXqsp7FaD6EDmbDwLbVCmtTCyFqOLkPr";
            _settings.AuthorizationPostUrl = "https://sandbox.paybright.com/CheckOut/ApplicationForm.aspx";
        }

        public ProcessPaymentResult Purchase(ProcessPaymentRequest paymentRequest)
        {
            // Compose request model 
            AuthorizationRequestModel requestModel = new AuthorizationRequestModel();
            // TODO: set request values based on shopping cart info
            #region Test Values
            // Hard code some values for now
            requestModel.x_account_id = "bUUBFD4FmSvkO3RTyJAmSvSBL6yOg2ZTCFXWuPDjdTuXKqanfg";
            requestModel.x_amount = 10.00m;
            requestModel.x_currency = "CAD";
            requestModel.x_customer_billing_address1 = "161 Bay Street";
            requestModel.x_customer_billing_address2 = "123";
            requestModel.x_customer_billing_city = "Toronto";
            requestModel.x_customer_billing_company = "PayBright";
            requestModel.x_customer_billing_country = "CA";
            requestModel.x_customer_billing_phone = "4161234567";
            requestModel.x_customer_billing_state = "ON";
            requestModel.x_customer_billing_zip = "N9B 2L1";
            requestModel.x_customer_email = "test@test.com";
            requestModel.x_customer_first_name = "John";
            requestModel.x_customer_last_name = "Smith";
            requestModel.x_customer_phone = "4161234567";
            requestModel.x_customer_shipping_address1 = "161 Bay Street";
            requestModel.x_customer_shipping_address2 = "123";
            requestModel.x_customer_shipping_city = "Toronto";
            requestModel.x_customer_shipping_company = "PayBright";
            requestModel.x_customer_shipping_country = "CA";
            requestModel.x_customer_shipping_first_name = "John";
            requestModel.x_customer_shipping_last_name = "Smith";
            requestModel.x_customer_shipping_phone = "4161234567";
            requestModel.x_customer_shipping_state = "ON";
            requestModel.x_customer_shipping_zip = "N9B 2L1";
            requestModel.x_description = "37";
            requestModel.x_invoice = "37";
            requestModel.x_locale = "EN";
            requestModel.x_platform = "custom";
            requestModel.x_reference = "37";
            requestModel.x_shop_country = "CA";
            requestModel.x_shop_name = "PayBright Demo Store";
            requestModel.x_test = "true";
            requestModel.x_url_callback = "https://www.buywell.com/";
            requestModel.x_url_cancel = "https://www.buywell.com/";
            requestModel.x_url_complete = "https://www.buywell.com/";

            #endregion
            // Calculate signature for intended post request
            requestModel.x_signature = SignatureHelper< AuthorizationRequestModel>.CalculateSignature(requestModel, _settings.ApiToken);
            // Send post request
            AuthorizationResponseModel responseModel = PayBrightAuthorizationPost(_settings.AuthorizationPostUrl, requestModel);

            // Compose payment result
            ProcessPaymentResult paymentResult = new ProcessPaymentResult();

            return paymentResult;
        }

        private AuthorizationResponseModel PayBrightAuthorizationPost(string authorizationUrl, AuthorizationRequestModel model)
        {
            RestClient client = new RestClient();

            var request = new RestRequest(authorizationUrl, Method.POST);

            request.AddParameter("x_test", model.x_test);
            request.AddParameter("x_reference", model.x_reference);
            request.AddParameter("x_account_id", model.x_account_id);
            request.AddParameter("x_amount", model.x_amount);
            request.AddParameter("x_currency", model.x_currency);
            request.AddParameter("x_url_callback", model.x_url_callback);
            request.AddParameter("x_url_complete", model.x_url_complete);
            request.AddParameter("x_url_cancel", model.x_url_cancel);
            request.AddParameter("x_shop_country", model.x_shop_country);
            request.AddParameter("x_shop_name", model.x_shop_name);
            request.AddParameter("x_customer_first_name", model.x_customer_first_name);
            request.AddParameter("x_customer_last_name", model.x_customer_last_name);
            request.AddParameter("x_customer_email", model.x_customer_email);
            request.AddParameter("x_customer_billing_country", model.x_customer_billing_country);
            request.AddParameter("x_customer_billing_city", model.x_customer_billing_city);
            request.AddParameter("x_customer_billing_address1", model.x_customer_billing_address1);
            request.AddParameter("x_customer_billing_state", model.x_customer_billing_state);
            request.AddParameter("x_customer_billing_zip", model.x_customer_billing_zip);
            request.AddParameter("x_customer_shipping_country", model.x_customer_shipping_country);
            request.AddParameter("x_customer_shipping_first_name", model.x_customer_shipping_first_name);
            request.AddParameter("x_customer_shipping_last_name", model.x_customer_shipping_last_name);
            request.AddParameter("x_customer_shipping_city", model.x_customer_shipping_city);
            request.AddParameter("x_customer_shipping_address1", model.x_customer_shipping_address1);
            request.AddParameter("x_customer_shipping_state", model.x_customer_shipping_state);
            request.AddParameter("x_customer_shipping_zip", model.x_customer_shipping_zip);
            request.AddParameter("x_description", model.x_description);
            request.AddParameter("x_signature", model.x_signature);

            AuthorizationResponseModel result = null;
            client.ExecuteAsync<AuthorizationResponseModel>(request, (response) =>
            {
                result = response.Data;
            });

            return result;
        }
    }
}
