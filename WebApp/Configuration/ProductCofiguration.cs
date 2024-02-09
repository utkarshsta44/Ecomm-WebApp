using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Models.Domain;

namespace WebApp.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.Property(x => x.ProductId).IsUnicode(true).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId);
            builder.HasOne(x => x.Carts).WithOne(x => x.Product).HasPrincipalKey<Product>(x => x.ProductId).IsRequired(false);

            builder.HasOne(x=>x.Favorite)
                .WithOne(x=>x.Products)
                .HasForeignKey<Product>(x=>x.ProductId)
                .HasPrincipalKey<Favourite>(x=>x.FId).IsRequired(false);
        }
    }
}