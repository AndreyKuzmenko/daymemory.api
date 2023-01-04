using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class NoteFileMapping : IEntityTypeConfiguration<NoteFile>
    {
        public void Configure(EntityTypeBuilder<NoteFile> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();

            builder.HasOne(x => x.File).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.NoteItem).WithMany(x => x.Files).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.FileId, x.NoteItemId });

            builder.ToTable("NoteImage");
        }
    }
}