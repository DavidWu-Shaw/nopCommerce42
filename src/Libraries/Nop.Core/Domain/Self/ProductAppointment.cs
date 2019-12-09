using System;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Self
{
    public class ProductAppointment : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Label { get; set; }
        public int StatusId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
