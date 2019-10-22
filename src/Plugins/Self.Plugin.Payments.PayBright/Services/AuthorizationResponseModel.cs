using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self.Plugin.Payments.PayBright.Services
{
    public class AuthorizationResponseModel : PayBrightModelBase
    {
        public string x_gateway_reference { get; set; }
    }
}
