using BeautyBook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyBook.Data.Configurations
{
    public class ServiceConfiguration
        : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasData(
                new Service
                {
                    Id = 1,
                    Name = "Дамско подстригване",
                    Description = "Професионално дамско подстригване",
                    Price = 30m,
                    DurationMinutes = 45,
                    CategoryId = 1
                },
                new Service
                {
                    Id = 2,
                    Name = "Боядисване",
                    Description = "Боядисване на коса",
                    Price = 80m,
                    DurationMinutes = 120,
                    CategoryId = 1
                },
                new Service
                {
                    Id = 3,
                    Name = "Сешоар",
                    Description = "Оформяне на косата със сешоар",
                    Price = 20m,
                    DurationMinutes = 30,
                    CategoryId = 1
                },
                new Service
                {
                    Id = 4,
                    Name = "Класически маникюр",
                    Description = "Оформяне и грижа за ноктите",
                    Price = 25m,
                    DurationMinutes = 45,
                    CategoryId = 2
                },
                new Service
                {
                    Id = 5,
                    Name = "Гел лак",
                    Description = "Маникюр с гел лак",
                    Price = 35m,
                    DurationMinutes = 60,
                    CategoryId = 2
                },
                new Service
                {
                    Id = 6,
                    Name = "Изграждане на нокти",
                    Description = "Изграждане и оформяне на ноктите",
                    Price = 60m,
                    DurationMinutes = 120,
                    CategoryId = 2
                },
                new Service
                {
                    Id = 7,
                    Name = "Дневен грим",
                    Description = "Лек грим за ежедневни поводи",
                    Price = 40m,
                    DurationMinutes = 45,
                    CategoryId = 3
                },
                new Service
                {
                    Id = 8,
                    Name = "Вечерен грим",
                    Description = "Професионален грим за специални поводи",
                    Price = 70m,
                    DurationMinutes = 60,
                    CategoryId = 3
                }
            );
        }
    }
}
