using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class MaterialQuantityHistoryMap : NopEntityTypeConfiguration<MaterialQuantityHistory>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public MaterialQuantityHistoryMap()
        {
            //this.ToTable("XrmsMaterialQuantityHistory");
            //this.HasKey(historyEntry => historyEntry.Id);

            //this.HasRequired(historyEntry => historyEntry.Material).WithMany().HasForeignKey(historyEntry => historyEntry.MaterialId).WillCascadeOnDelete(true);
        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<MaterialQuantityHistory> builder)
        {
            builder.ToTable("XrmsMaterialQuantityHistory");
            builder.HasKey(q => q.Id);
            builder.HasOne(q => q.Material).WithMany().HasForeignKey(q => q.MaterialId).IsRequired();
            base.Configure(builder);
        }

        #endregion // Methods
    }
}
