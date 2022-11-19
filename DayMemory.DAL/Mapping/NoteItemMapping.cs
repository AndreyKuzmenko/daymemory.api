using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class NoteItemMapping : IEntityTypeConfiguration<NoteItem>
    {
        public void Configure(EntityTypeBuilder<NoteItem> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.Location).WithOne(x => x.NoteItem).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("NoteItem");
        }
    }
}