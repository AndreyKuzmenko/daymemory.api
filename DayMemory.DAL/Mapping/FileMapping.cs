using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class FileMapping : IEntityTypeConfiguration<Core.Models.Personal.File>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Personal.File> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(50);

            builder.HasKey(c => c.Id);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();

            builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.FileSize).IsRequired();
            builder.Property(x => x.FileContentType).HasMaxLength(255);

            builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Image");
        }
    }
}

