using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyBook.Data.Configurations
{
    public class EmployeeConfiguration
        : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .HasOne(e => e.Category)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                 .HasMany(e => e.Appointments)
                 .WithOne(a => a.Employee)
                 .HasForeignKey(a => a.EmployeeId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Мария",
                    LastName = "Иванова",
                    Position = "Стилист колорист",
                    Bio = "Специалист по боядисване и сложни цветови техники.",
                    CategoryId = 1
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Десислава",
                    LastName = "Георгиева",
                    Position = "Фризьор",
                    Bio = "Подстригване, оформяне и грижа за косата.",
                    CategoryId = 1
                },

                
                new Employee
                {
                    Id = 3,
                    FirstName = "Елена",
                    LastName = "Петрова",
                    Position = "Маникюрист",
                    Bio = "Класически маникюр и маникюр с гел лак.",
                    CategoryId = 2
                },
                new Employee
                {
                    Id = 4,
                    FirstName = "Виктория",
                    LastName = "Стоянова",
                    Position = "Нокътен техник",
                    Bio = "Изграждане и дизайн на нокти.",
                    CategoryId = 2
                },

                
                new Employee
                {
                    Id = 5,
                    FirstName = "Ралица",
                    LastName = "Димитрова",
                    Position = "Гримьор",
                    Bio = "Дневен и ежедневен грим.",
                    CategoryId = 3
                },
                new Employee
                {
                    Id = 6,
                    FirstName = "Калина",
                    LastName = "Тодорова",
                    Position = "Визажист",
                    Bio = "Вечерен и официален грим за специални поводи.",
                    CategoryId = 3
                }
            );
        }
    }
}