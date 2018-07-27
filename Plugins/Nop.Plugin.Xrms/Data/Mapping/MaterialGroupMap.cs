using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class MaterialGroupMap : NopEntityTypeConfiguration<MaterialGroup>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public MaterialGroupMap()
        {
            //this.ToTable("XrmsMaterialGroup");
            //this.HasKey(c => c.Id);
            //this.Property(c => c.Name).IsRequired().HasMaxLength(400);
        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<MaterialGroup> builder)
        {

            builder.ToTable("XrmsMaterialGroup");
            builder.HasKey(mg => mg.Id);
            builder.Property(mg => mg.Name).IsRequired().HasMaxLength(400);

            base.Configure(builder);
        }

        #endregion // Methods
    }
}