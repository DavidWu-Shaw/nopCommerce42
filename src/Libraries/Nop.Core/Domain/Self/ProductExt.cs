using System.Collections.Generic;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Self;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.Catalog
{
    public partial class Product : BaseEntity, ILocalizedEntity, ISlugSupported, IAclSupported, IStoreMappingSupported, IDiscountSupported
    {
        private ICollection<ProductAppointment> _productAppointments;

        public virtual ICollection<ProductAppointment> ProductAppointments
        {
            get { return _productAppointments ?? (_productAppointments = new List<ProductAppointment>()); }
            protected set { _productAppointments = value; }
        }
    }
}

