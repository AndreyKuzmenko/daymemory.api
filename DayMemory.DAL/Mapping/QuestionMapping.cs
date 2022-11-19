using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class QuestionMapping : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.Text).HasMaxLength(1000);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();
            
            builder.HasOne(x => x.QuestionList).WithMany(x => x.Questions).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Question");
        }
    }
}