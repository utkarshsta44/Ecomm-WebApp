using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Models.Domain;

namespace WebApp.Configuration
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.ToTable("Favourites");
            builder.HasKey(x => x.FId);
            builder.HasOne(x => x.Products)
                .WithOne(x => x.Favorite)
                .IsRequired(false);
                
        }
    }
}
