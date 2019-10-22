using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self.Plugin.Payments.PayBright.Services
{
    public class PayBrightModelBase
    {
        public string x_account_id { get; set; }
        public string x_reference { get; set; }
        public string x_signature { get; set; }
        public string x_test { get; set; }
    }
}
