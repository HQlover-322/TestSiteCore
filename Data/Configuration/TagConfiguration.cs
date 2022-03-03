using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEST.Data.Entities;

namespace TEST.Data.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(10);
        }
    }
}
