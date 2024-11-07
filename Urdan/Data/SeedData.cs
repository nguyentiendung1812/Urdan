using Microsoft.EntityFrameworkCore;
using Urdan.Models;

namespace Urdan.Data
{
  public class SeedData
  {
    public static void Seed(IServiceProvider serviceProvider)
    {
      using (UrdanContext context = new UrdanContext(serviceProvider.GetRequiredService<DbContextOptions<UrdanContext>>()))
      {

        // Category
        if (!context.Categories.Any())
        {
          context.Categories.AddRange(
            new Category
            {
              Name = "Sofa"
            },
            new Category
            {
              Name = "Chair"
            },
            new Category
            {
              Name = "Decoration"
            },
            new Category
            {
              Name = "Bookshelf"
            },
            new Category
            {
              Name = "Table"
            },
            new Category
            {
              Name = "Wardrobe"
            }
        );
        }


        // User
        if (!context.Users.Any(u => u.Role == Role.Admin))
        {
          string password = "123456";
          string passwordHash = BC.HashPassword(password, BC.GenerateSalt(10));
          context.Users.Add(new User { Username = "admin", Email = "admin@gmail.com", Password = passwordHash, Role = Role.Admin });
        }

        // Brand
        if (!context.Brands.Any())
        {
          context.Brands.AddRange(
      new Brand { Name = "Aillen" }, new Brand { Name = "ALICE" }, new Brand { Name = "ARABICA" }, new Brand { Name = "AURORA" }, new Brand { Name = "BELLA" }, new Brand { Name = "Binas" }, new Brand { Name = "Euro" }, new Brand { Name = "Hobu" }, new Brand { Name = "Luxury" }, new Brand { Name = "Miso" }, new Brand { Name = "Poplar" }, new Brand { Name = "Tabu" }, new Brand { Name = "Woody" }
      );
        }

        context.SaveChanges();
      }
    }
  }
}
