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

        public ProcessPaymentResult ChargeWithVault(ProcessPaymentRequest paymentRequest)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();
            string dataKey = paymentRequest.SavedCardVault;

            try
            {
                if (paymentRequest.IsNewCardSaveAllowed)
                {
                    // Vault Add card
                    string expireDate = GetExpireDate(paymentRequest.CreditCardExpireYear, paymentRequest.CreditCardExpireMonth);
                    dataKey = AddCardToVault(paymentRequest.CreditCardNumber, expireDate, paymentRequest.CustomerId.ToString());
                }
                // Create purchase
                ResPurchaseCC purchase = new ResPurchaseCC();
                purchase.SetDataKey(dataKey);

                purchase.SetOrderId(paymentRequest.OrderGuid.ToString());
                purchase.SetAmount(paymentRequest.OrderTotal.ToString());
                purchase.SetCryptType(_monerisPaymentSettings.Crypt);
                purchase.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
                // CVD check
                CvdInfo cvdCheck = new CvdInfo();
                cvdCheck.SetCvdIndicator("1");
                cvdCheck.SetCvdValue(paymentRequest.CreditCardCvv2);
                purchase.SetCvdInfo(cvdCheck);

                Receipt receipt = PerformTransation(purchase);
                result = ConvertToResult(receipt);
            }
            catch (Exception ex)
            {
                result.NewPaymentStatus = PaymentStatus.Pending;
                result.AddError(ex.Message);
            }

            return result;
        }

        public string AddCardToVault(string creditCardNumber, string expireDate, string customerID)
        {
            ResAddCC resaddcc = new ResAddCC();
            resaddcc.SetPan(creditCardNumber);
            resaddcc.SetExpDate(expireDate);
            resaddcc.SetCustId(customerID);
            resaddcc.SetCryptType(_monerisPaymentSettings.Crypt);
            resaddcc.SetGetCardType("true");

            Receipt receipt = PerformTransation(resaddcc);
            string dataKey = receipt.GetDataKey();

            return dataKey;
        }

        public ProcessPaymentResult Charge(ProcessPaymentRequest paymentRequest)
        {
            // Create purchase
            Purchase purchase = new Purchase();
            purchase.SetPan(paymentRequest.CreditCardNumber);
            // Expire date "YYMM" format
            string expireDate = GetExpireDate(paymentRequest.CreditCardExpireYear, paymentRequest.CreditCardExpireMonth);
            purchase.SetExpDate(expireDate);

            purchase.SetOrderId(paymentRequest.OrderGuid.ToString());
            purchase.SetAmount(paymentRequest.OrderTotal.ToString());
            purchase.SetCustId(paymentRequest.CustomerId.ToString());
            purchase.SetCryptType(_monerisPaymentSettings.Crypt);
            purchase.SetDynamicDescriptor(_monerisPaymentSettings.DynamicDescriptor);
            // CVD check
            CvdInfo cvdCheck = new CvdInfo();
            cvdCheck.SetCvdIndicator("1");
            cvdCheck.SetCvdValue(paymentRequest.CreditCardCvv2);
            purchase.SetCvdInfo(cvdCheck);

            ProcessPaymentResult result = new ProcessPaymentResult();
            try
            {
                Receipt receipt = PerformTransation(purchase);
                result = ConvertToResult(receipt);
            }
            catch (Exception ex)
            {
                result.NewPaymentStatus = PaymentStatus.Pending;
                result.AddError(ex.Message);
            }

            return result;
        }

        public ProcessPaymentResult Authorize(ProcessPaymentRequest paymentRequest)
        {
            PreAuth preauth = new PreAuth();
            preauth.SetOrderId(paymentRequest.InitialOrderId.ToString());
            preauth.SetAmount(paymentRequest.OrderTotal.ToString());
            preauth.SetPan(paymentRequest.CreditCardNumber);
            // Expire date "YYMM" format
            string expireDate = string.Format("{0:D2}{1:D2}", paymentRequest.CreditCardExpireYear, paymentRequest.CreditCardExpireMonth);
            preauth.SetExpDate(expireDate);
            preauth.SetCryptType(_monerisPaymentSettings.Crypt);

            ProcessPaymentResult result = new ProcessPaymentResult();
            try
            {
                Receipt receipt = PerformTransation(preauth);
                result = ConvertToResult(receipt);
            }
            catch (Exception ex)
            {
                result.NewPaymentStatus = PaymentStatus.Pending;
                result.AddError(ex.Message);
            }

            return result;
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException();
        }

        private Receipt PerformTransation(Transaction transaction)
        {
            _transactionRequest.SetTransaction(transaction);
            _transactionRequest.Send();

            return _transactionRequest.GetReceipt();
        }

        private ProcessPaymentResult ConvertToResult(Receipt receipt)
        {
            ProcessPaymentResult result = new ProcessPaymentResult();

            string CardType = receipt.GetCardType();
            string TransAmount = receipt.GetTransAmount();
            string TxnNumber = receipt.GetTxnNumber();
            string ReceiptId = receipt.GetReceiptId();
            string TransType = receipt.GetTransType();
            string ReferenceNum = receipt.GetReferenceNum();
            string ResponseCode = receipt.GetResponseCode();
            string ISO = receipt.GetISO();
            string BankTotals = receipt.GetBankTotals();
            string Message = receipt.GetMessage();
            string AuthCode = receipt.GetAuthCode();
            string Complete = receipt.GetComplete();
            string TransDate = receipt.GetTransDate();
            string TransTime = receipt.GetTransTime();
            string Ticket = receipt.GetTicket();
            string TimedOut = receipt.GetTimedOut();
            string IsVisaDebit = receipt.GetIsVisaDebit();
            string HostId = receipt.GetHostId();
            string IssuerId = receipt.GetIssuerId();

            return result;
        }

        private string GetExpireDate(int year, int month)
        {
            return string.Format("{0:D2}{1:D2}", year, month);
        }
    }
}
