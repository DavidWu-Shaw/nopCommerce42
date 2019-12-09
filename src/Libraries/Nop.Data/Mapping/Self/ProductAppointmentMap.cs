using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Self;

namespace Nop.Data.Mapping.Self
{
    public partial class ProductAppointmentMap : NopEntityTypeConfiguration<ProductAppointment>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductAppointment> builder)
        {
            builder.ToTable("tblProductAppointment");
            builder.HasKey(productAppointment => productAppointment.Id);

            builder.HasOne(productAppointment => productAppointment.Product)
                .WithMany(product => product.ProductAppointments)
                .HasForeignKey(productAppointment => productAppointment.ProductId)
                .IsRequired();

            builder.HasOne(productAppointment => productAppointment.Customer)
                .WithMany()
                .HasForeignKey(productAppointment => productAppointment.CustomerId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}
