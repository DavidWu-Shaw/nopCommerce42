using Nop.Core.Configuration;

namespace Self.Plugin.Payments.PayBright
{
    /// <summary>
    /// Represents a Moneris payment settings
    /// </summary>
    public class PaymentPayBrightSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a SecureNet identifier
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets a key for connecting with PayBright APIs 
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Gets or sets a secure key
        /// </summary>
        public string ApiToken { get; set; }
        public string AuthorizationPostUrl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use sandbox (testing environment)
        /// </summary>
        public bool UseSandbox { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to validate billing address
        /// </summary>
        public bool ValidateAddress { get; set; }
        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        /// <summary>
        /// Key used to deploy Lambda automatically?
        /// </summary>
        public string AwsKey { get; set; }

        /// <summary>
        /// dynamic_descriptor
        /// </summary>
        public string DynamicDescriptor { get; set; }

        /// <summary>
        /// Crypt
        /// </summary>
        public string Crypt { get; set; }

        /// <summary>
        /// PublicKey
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Use aws lambda otherwise use task scheduler? failure mode possible.
        /// </summary>
        public bool UseLambdaForRecurringPayment { get; set; }
        public object AwsSecretKey { get; internal set; }
        public object AwsAuthenticationKey { get; internal set; }
    }
}