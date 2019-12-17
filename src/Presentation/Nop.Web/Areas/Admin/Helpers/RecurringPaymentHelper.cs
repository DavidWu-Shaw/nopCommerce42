using System;
using Nop.Core.Domain.Catalog;

namespace Nop.Web.Areas.Admin.Helpers
{
    public static class RecurringPaymentHelper
    {
        public static DateTime GetNexDate(DateTime startDate, RecurringProductCyclePeriod cyclePeriod)
        {
            DateTime today = DateTime.Today;
            DateTime nextDate = today.AddDays(1);
            switch (cyclePeriod)
            {
                case RecurringProductCyclePeriod.Months:
                    var months = 12 * (today.Year - startDate.Year) + today.Month - startDate.Month;
                    if (today.Date >= startDate.Date)
                    {
                        months++;
                    }
                    nextDate = startDate.AddMonths(months);
                    break;
                case RecurringProductCyclePeriod.Years:
                    var years = (today - startDate).TotalDays / 365.25;
                    nextDate = startDate.AddYears((int)Math.Ceiling(years));
                    break;
                case RecurringProductCyclePeriod.Weeks:
                    var days = (today - startDate).TotalDays;
                    nextDate = startDate.AddDays(days);
                    break;
            }

            return nextDate;
        }
    }
}
