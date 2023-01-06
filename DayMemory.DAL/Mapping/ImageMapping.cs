using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayMemory.DAL.Mapping
{
    public class ImageMapping : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Personal.Image> builder)
        {
            builder.ToTable("Image");
        }
    }
}

