using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEST.Data.Entities;

namespace TEST.Data.Configuration
{
    public class ArtricleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(1024);
            builder.Property(x=>x.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("getdate()");
            builder.HasIndex(x=>x.CreatedAt);
            builder.HasIndex(x=>x.UpdatedAt);
            builder.HasOne(x=>x.HeroImage).WithOne(x=>x.Article).HasForeignKey<HeroImage>(x=>x.ArticleId);
        }
    }
}
