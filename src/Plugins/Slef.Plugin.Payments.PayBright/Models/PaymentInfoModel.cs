using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Self.Plugin.Payments.PayBright.Models
{
    /// <summary>
    /// Represents the PayBright payment model
    /// </summary>
    public class PaymentInfoModel : BaseNopModel
    {
        #region Ctor

        public PaymentInfoModel()
        {
            ExpireMonths = new List<SelectListItem>();
            ExpireYears = new List<SelectListItem>();
            CardTypes = new List<SelectListItem>();
            
        }

        #endregion

        #region Properties

        public bool IsGuest { get; set; }
        public string CardNumber { get; set; }    
        public string CardCode { get; set; } 
        public string ExpireMonth { get; set; }
        public IList<SelectListItem> ExpireMonths { get; set; }
        public string ExpireYear { get; set; }
        public IList<SelectListItem> ExpireYears { get; set; }
        public string CardType { get; set; }
        public IList<SelectListItem> CardTypes { get; set; }
        public string Errors { get; set; }

 

        #endregion
    }
}