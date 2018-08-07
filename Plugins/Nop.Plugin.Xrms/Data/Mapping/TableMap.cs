using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class TableMap : NopEntityTypeConfiguration<Table>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public TableMap()
        {

        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Table> builder)
        {

            builder.ToTable("XrmsTable");
            builder.HasKey(tbl => tbl.Id);
            builder.Property(tbl => tbl.Name).IsRequired().HasMaxLength(400);
            builder.HasOne(tbl => tbl.CurrentOrder).WithOne(ord => ord.Table).HasForeignKey<CurrentOrder>(o => o.TableId).OnDelete(DeleteBehavior.Restrict);
            base.Configure(builder);
        }

        #endregion // Methods
    }
}