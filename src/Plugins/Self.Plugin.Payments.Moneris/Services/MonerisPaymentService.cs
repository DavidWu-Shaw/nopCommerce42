using System;
using Moneris;
using Nop.Core.Domain.Payments;
using Nop.Services.Payments;

namespace Self.Plugin.Payments.Moneris.Services
{
    public class MonerisPaymentService : IPaymentGatewayService
    {
        private MonerisPaymentSettings _monerisPaymentSettings { get; set; }
        private HttpsPostRequest _transactionRequest { get; set; }

        public MonerisPaymentService(MonerisPaymentSettings monerisPaymentSettings)
        {
            _monerisPaymentSettings = monerisPaymentSettings;

            _transactionRequest = new HttpsPostRequest();
            _transactionRequest.SetProcCountryCode(_monerisPaymentSettings.ProcessingCountryCode);
            _transactionRequest.SetTestMode(_monerisPaymentSettings.UseSandbox);
            _transactionRequest.SetStoreId(_monerisPaymentSettings.StoreId);
            _transactionRequest.SetApiToken(_monerisPaymentSettings.ApiToken);
        }

        public ProcessPaymentResult Charge(ProcessPaymentRequest paymentRequest)
        {
            var result = new ProcessPaymentResult();
            var dataKey = paymentRequest.CustomValues["SavedCardVault"] as string;
            var isNewCardSaveAllowed = System.Convert.ToBoolean(paymentRequest.CustomValues["IsNewCardSaveAllowed"]);
            // Expire date "YYMM" format
            var expireDate = GetExpireDate(paymentRequest.CreditCardExpireYear, paymentRequest.CreditCardExpireMonth);

            try
            {
                if (isNewCardSaveAllowed)
                {
                    // Vault Add card
                    dataKey = AddCardToVault(paymentRequest.CreditCardNumber, expireDate, paymentRequest.CustomerId.ToString());
                    // TODO: save into database

                }
                Receipt receipt = null;
                if (!string.IsNullOrEmpty(dataKey))
                {
                    // Create purchase with dataKey
                    var purchase = CreatePurchaseWithVault(dataKey, paymentRequest.OrderGuid.ToString(),
                        paymentRequest.OrderTotal.ToString(), paymentRequest.CreditCardCvv2);

                    receipt = PerformTransation(purchase);
                }
                else
                {
                    // Create purchase
                    var purchase = CreatePurchase(paymentRequest.CreditCardNumber, expireDate, paymentRequest.CustomerId.ToString(), 
                        paymentRequest.OrderGuid.ToString(), paymentRequest.OrderTotal.ToString(), paymentRequest.CreditCardCvv2);

                    receipt = PerformTransation(purchase);
                }
                result = ConvertToChargeResult(receipt);
            }
            catch (Exception ex)
            {
                result.NewPaymentStatus = PaymentStatus.Pending;
                result.AddError(ex.Message);
            }

            return result;
        }

        public string AddCardToVault(string creditCardNumber, string expireDate, string customerId)
        {
            var resaddcc = new ResAddCC();
            resaddcc.SetPan(creditCardNumber);
            resaddcc.SetExpDate(expireDate);
            resaddcc.SetCustId(customerId);
            resaddcc.SetCryptType(_monerisPaymentSettings.Crypt);
            resaddcc.SetGetCardType("true");

            var receipt = PerformTransation(resaddcc);
            var dataKey = receipt.GetDataKey();

            return dataKey;
        }

        private ResPurchaseCC CreatePurchaseWithVault(string dataKey, string orderId, string amount, string cvd)
        {
            // Create purchase with dataKey
            var purchase = new ResPurchaseCC();
            purchase.SetDataKey(dataKey);
            purchase.SetOrderId(orderId);
            purchase.SetAmount(amount);
            purchase.SetCryptType(_monerisPaymentSettings.Crypt);
            purchase.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
            // CVD check
            purchase.SetCvdInfo(CreateCvdInfo(cvd));

            return purchase;
        }

        private Purchase CreatePurchase(string cardNumber, string expireDate, string customerId, string orderId, string amount, string cvd)
        {
            // Create purchase
            var purchase = new Purchase();
            purchase.SetPan(cardNumber);
            // Expire date "YYMM" format
            purchase.SetExpDate(expireDate);
            purchase.SetOrderId(orderId);
            purchase.SetCustId(customerId);
            purchase.SetAmount(amount);
            purchase.SetCryptType(_monerisPaymentSettings.Crypt);
            purchase.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
            // CVD check
            purchase.SetCvdInfo(CreateCvdInfo(cvd));

            return purchase;
        }

        private ResPreauthCC CreateAuthWithVault(string dataKey, string orderId, string amount, string cvd)
        {
            // Create purchase with dataKey
            var auth = new ResPreauthCC();
            auth.SetDataKey(dataKey);
            auth.SetOrderId(orderId);
            auth.SetAmount(amount);
            auth.SetCryptType(_monerisPaymentSettings.Crypt);
            auth.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
            // CVD check
            auth.SetCvdInfo(CreateCvdInfo(cvd));

            return auth;
        }

        private PreAuth CreateAuth(string cardNumber, string expireDate, string customerId, string orderId, string amount, string cvd)
        {
            // Create purchase
            var auth = new PreAuth();
            auth.SetPan(cardNumber);
            // Expire date "YYMM" format
            auth.SetExpDate(expireDate);
            auth.SetOrderId(orderId);
            auth.SetCustId(customerId);
            auth.SetAmount(amount);
            auth.SetCryptType(_monerisPaymentSettings.Crypt);
            auth.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
            // CVD check
            auth.SetCvdInfo(CreateCvdInfo(cvd));

            return auth;
        }

        private CvdInfo CreateCvdInfo(string cvd)
        {
            CvdInfo cvdCheck = new CvdInfo();
            cvdCheck.SetCvdIndicator("1");
            cvdCheck.SetCvdValue(cvd);
            return cvdCheck;
        }

        public ProcessPaymentResult Authorize(ProcessPaymentRequest paymentRequest)
        {
            var result = new ProcessPaymentResult();

            var dataKey = paymentRequest.CustomValues["SavedCardVault"] as string;
            var isNewCardSaveAllowed = System.Convert.ToBoolean(paymentRequest.CustomValues["IsNewCardSaveAllowed"]);
            // Expire date "YYMM" format
            var expireDate = GetExpireDate(paymentRequest.CreditCardExpireYear, paymentRequest.CreditCardExpireMonth);

            try
            {
                if (isNewCardSaveAllowed)
                {
                    // Vault Add card
                    dataKey = AddCardToVault(paymentRequest.CreditCardNumber, expireDate, paymentRequest.CustomerId.ToString());
                    // TODO: save into database

                }
                Receipt receipt = null;
                if (!string.IsNullOrEmpty(dataKey))
                {
                    // Create auth with dataKey
                    var auth = CreateAuthWithVault(dataKey, paymentRequest.OrderGuid.ToString(),
                        paymentRequest.OrderTotal.ToString(), paymentRequest.CreditCardCvv2);

                    receipt = PerformTransation(auth);
                }
                else
                {
                    // Create auth
                    var auth = CreateAuth(paymentRequest.CreditCardNumber, expireDate, paymentRequest.CustomerId.ToString(),
                        paymentRequest.OrderGuid.ToString(), paymentRequest.OrderTotal.ToString(), paymentRequest.CreditCardCvv2);

                    receipt = PerformTransation(auth);
                }

                result = ConvertToAuthResult(receipt);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public ProcessPaymentResult Capture(string orderId, string amount, string authTransactionNumber)
        {
            var result = new ProcessPaymentResult();

            Completion completion = new Completion();
            completion.SetOrderId(orderId);
            completion.SetCompAmount(amount);
            completion.SetTxnNumber(authTransactionNumber);
            completion.SetCryptType(_monerisPaymentSettings.Crypt);
            completion.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);

            try
            {
                Receipt receipt = PerformTransation(completion);

            }
            catch(Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        private Receipt PerformTransation(Transaction transaction)
        {
            _transactionRequest.SetTransaction(transaction);
            _transactionRequest.Send();

            return _transactionRequest.GetReceipt();
        }

        private ProcessPaymentResult ConvertToChargeResult(Receipt receipt)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();

            if (receipt.GetComplete() == "false")
            {
                result.AddError("Payment is not successful. Please try again or check payment option.");
            }
            result.AvsResult = receipt.GetAvsResultCode();
            result.Cvv2Result = receipt.GetCavvResultCode();
            result.AuthorizationTransactionId = receipt.GetReferenceNum();
            result.AuthorizationTransactionCode = receipt.GetAuthCode();
            result.AuthorizationTransactionResult = receipt.GetResponseCode();
            result.CaptureTransactionId = receipt.GetTxnNumber(); // required for refunds
            result.CaptureTransactionResult = receipt.GetTransactionId();
            if (receipt.GetResponseCode() != null || receipt.GetResponseCode() != "null")
            {
                int responseCode = Int32.Parse(receipt.GetResponseCode().ToString());
                //Moneris reference
                //Transaction Response Code < 50: Transaction approved >= 50: Transaction declined 
                //NULL: Transaction was not sent for authorization For further details on the response codes that are returned please see the Response Codes table
                if (responseCode < 50)
                {
                    result.NewPaymentStatus = PaymentStatus.Paid;
                }
            }

            return result;
        }

        private ProcessPaymentResult ConvertToAuthResult(Receipt receipt)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();

            if (receipt.GetComplete() == "false")
            {
                result.AddError("Authorization is not successful. Please try again or check payment option.");
            }
            result.AvsResult = receipt.GetAvsResultCode();
            result.Cvv2Result = receipt.GetCavvResultCode();
            result.AuthorizationTransactionId = receipt.GetTxnNumber();
            result.AuthorizationTransactionCode = receipt.GetAuthCode();
            result.AuthorizationTransactionResult = receipt.GetResponseCode();

            return result;
        }

        private string GetExpireDate(int year, int month)
        {
            return string.Format("{0:D2}{1:D2}", year, month);
        }
    }
}
