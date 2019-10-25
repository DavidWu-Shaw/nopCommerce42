using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self.Plugin.Payments.PayBright.Services
{
    public class AuthorizationRequestModel : PayBrightModelBase
    {
        public decimal x_amount { get; set; }
        public string x_currency { get; set; }
        public string x_customer_billing_address1 { get; set; }
        public string x_customer_billing_address2 { get; set; }
        public string x_customer_billing_city { get; set; }
        public string x_customer_billing_company { get; set; }
        public string x_customer_billing_country { get; set; }
        public string x_customer_billing_phone { get; set; }
        public string x_customer_billing_state { get; set; }
        public string x_customer_billing_zip { get; set; }
        public string x_customer_email { get; set; }
        public string x_customer_first_name { get; set; }
        public string x_customer_last_name { get; set; }
        public string x_customer_phone { get; set; }

        public string x_customer_shipping_address1 { get; set; }
        public string x_customer_shipping_address2 { get; set; }
        public string x_customer_shipping_city { get; set; }
        public string x_customer_shipping_company { get; set; }
        public string x_customer_shipping_country { get; set; }
        public string x_customer_shipping_first_name { get; set; }
        public string x_customer_shipping_last_name { get; set; }
        public string x_customer_shipping_phone { get; set; }
        public string x_customer_shipping_state { get; set; }
        public string x_customer_shipping_zip { get; set; }
        public string x_description { get; set; }
        public string x_invoice { get; set; }
        public string x_locale { get; set; }
        public string x_platform { get; set; }
        public string x_shop_country { get; set; }
        public string x_shop_name { get; set; }
        public string x_url_callback { get; set; }
        public string x_url_cancel { get; set; }
        public string x_url_complete { get; set; }
        public string x_token { get; set; }
    }
}
