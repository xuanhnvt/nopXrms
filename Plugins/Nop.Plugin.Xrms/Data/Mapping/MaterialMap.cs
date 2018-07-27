
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class MaterialMap : NopEntityTypeConfiguration<Material>
    {
        #region Ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public MaterialMap()
        {
            //this.ToTable("XrmsMaterial");
            //this.HasKey(c => c.Id);
            //this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            //this.HasRequired(c => c.MaterialGroup).WithMany(g => g.Materials).HasForeignKey(c => c.MaterialGroupId);
        }
        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("XrmsMaterial");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(400);
            builder.HasOne(m => m.MaterialGroup).WithMany(g => g.Materials).HasForeignKey(m => m.MaterialGroupId).IsRequired();
            base.Configure(builder);
        }

        #endregion // Methods
    }
}
