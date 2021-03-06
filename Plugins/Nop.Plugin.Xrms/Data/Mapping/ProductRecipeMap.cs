
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Data.Mapping
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class ProductRecipeMap : NopEntityTypeConfiguration<ProductRecipe>
    {
        #region Ctors

        /// <summary>
        /// Ctor
        /// </summary>
        public ProductRecipeMap()
        {
            //this.ToTable("XrmsProductRecipe");
            //this.HasKey(pr => pr.Id);
            //this.HasRequired(pr => pr.Material).WithMany().HasForeignKey(pr => pr.MaterialId);
        }

        #endregion // Ctors

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductRecipe> builder)
        {
            builder.ToTable("XrmsProductRecipe");
            builder.HasKey(pr => pr.Id);
            builder.HasOne(pr => pr.Material)
                .WithMany()
                .HasForeignKey(pr => pr.MaterialId).IsRequired();
            base.Configure(builder);
        }

        #endregion // Methods
    }
}