using System;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Self
{
    public class Appointment : BaseEntity
    {
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string Label { get; set; }
        public int StatusId { get; set; }
        public int ResourcetId { get; set; }
        public int CustomerId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
