using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class NoteImageMapping : IEntityTypeConfiguration<NoteImage>
    {
        public void Configure(EntityTypeBuilder<NoteImage> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();

            builder.HasOne(x => x.Image).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.NoteItem).WithMany(x => x.Images).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.ImageId, x.NoteItemId });

            builder.ToTable("NoteImage");
        }
    }
}