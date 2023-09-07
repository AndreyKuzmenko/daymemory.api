using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class NoteTagMapping : IEntityTypeConfiguration<NoteTag>
    {
        public void Configure(EntityTypeBuilder<NoteTag> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();

            builder.HasOne(x => x.Tag).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.NoteItem).WithMany(x => x.Tags).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.TagId, x.NoteItemId });

            builder.ToTable("NoteTag");
        }
    }
}