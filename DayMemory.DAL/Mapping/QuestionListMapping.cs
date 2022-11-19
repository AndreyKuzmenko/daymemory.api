using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class QuestionListMapping : IEntityTypeConfiguration<QuestionList>
    {
        public void Configure(EntityTypeBuilder<QuestionList> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.Text).HasMaxLength(1000);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();
            builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("QuestionList");
        }
    }
}