
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CurrentOrderMap : NopEntityTypeConfiguration<CurrentOrder>
    {
        #region Ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public CurrentOrderMap()
        {

        }
        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CurrentOrder> builder)
        {
            builder.ToTable("XrmsCurrentOrder");
            builder.HasKey(order => order.Id);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}
