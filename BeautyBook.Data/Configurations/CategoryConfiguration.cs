using BeautyBook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyBook.Data.Configurations
{
    public class CategoryConfiguration
        : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Фризьорство"
                },
                new Category
                {
                    Id = 2,
                    Name = "Маникюр"
                },
                new Category
                {
                    Id = 3,
                    Name = "Грим"
                }
            );
        }
    }
}
