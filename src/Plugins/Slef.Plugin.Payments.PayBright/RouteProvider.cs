using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Self.Plugin.Payments.PayBright
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Plugin.Payments.PayBright.CompleteHandler", "Plugins/PayBright/CompletePaymentHandler",
                 new { controller = "PaymentPayBright", action = "CompletePaymentHandler" });

            routeBuilder.MapRoute("Plugin.Payments.PayBright.CancelHandler", "Plugins/PayBright/CancelPaymentHandler",
                 new { controller = "PaymentPayBright", action = "CancelPaymentHandler" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return -1; }
        }
    }
}
