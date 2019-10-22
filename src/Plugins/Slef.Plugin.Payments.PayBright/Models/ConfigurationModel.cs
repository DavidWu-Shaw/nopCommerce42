using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Self.Plugin.Payments.PayBright.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.StoreId")]
        public string StoreId { get; set; }
        public bool StoreId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.ApiToken")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string ApiToken { get; set; }
        public bool ApiToken_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandbox_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.ValidateAddress")]
        public bool ValidateAddress { get; set; }
        public bool ValidateAddress_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.DynamicDescriptor")]
        public string DynamicDescriptor { get; set; }
        public bool DynamicDescriptor_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.Crypt")]
        public string Crypt { get; set; }
        public bool Crypt_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.PublicKey")]
        public string PublicKey { get; set; }
        public bool PublicKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.AwsAuthenticationKey")]
        public string AwsKey { get; set; }
        public bool AwsKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayBright.Fields.AwsSecretKey")]
        public object AwsSecretKey { get; internal set; }
        public bool AwsSecretKey_OverrideForStore { get; internal set; }
    }
}
