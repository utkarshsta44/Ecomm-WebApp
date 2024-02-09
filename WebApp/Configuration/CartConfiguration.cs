using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Domain;


namespace WebApp.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.Property(z => z.CartId).IsUnicode(true).ValueGeneratedOnAdd();
            builder.HasOne(x => x.User)
                .WithOne(x => x.Carts)
                .HasForeignKey<Cart>(c => c.UserId1)
                .IsRequired(false);

            builder.HasOne(x => x.Product)
                .WithOne(x => x.Carts)
                .HasForeignKey<Cart>(x => x.ProductId)
                .IsRequired(false);


        }
    }
}