using System.Linq;
using System.Threading.Tasks;

// dodano
using RezerwacjaSal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace RezerwacjaSal.Data
{
    // Inicjalizacja bazy danych jakimiś przykładowymi danymi 
    public class DbInitializer
    {
        public DbInitializer(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static IConfiguration Configuration { get; set; }

        public static async Task InitializeAsync(
            RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DbInitializer> logger)
        {

            context.Database.EnsureCreated();

            // Sprawdź czy są jakieś wydziały
            if (context.Departments.Any())
            {
                return;   // są więc nie nadpisuj bazy danych 
            }



            var adminRole = new IdentityRole
            {
                Id = "0",
                Name = "administrator"
            };

            var resultDefaultAdminRole = await roleManager.CreateAsync(adminRole);
            if (resultDefaultAdminRole.Succeeded) logger.LogInformation("Utworzono rolę administratora.");

            var userRole = new IdentityRole
            {
                Id = "1",
                Name = "użytkownik"
            };

            var resultDefaultUserRole = await roleManager.CreateAsync(userRole);
            if (resultDefaultUserRole.Succeeded) logger.LogInformation("Utworzono rolę użytkownika.");
            
            var newAdmin = new ApplicationUser
            {
                UserName = Configuration.GetConnectionString("AdminLogin"),
                Email = Configuration.GetConnectionString("AdminLogin"),
                FirstName = "Admin",
                LastName = "Admin",
                Note = "Administrator systemu Bulbulator",
                Number = 1,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Employment = "Administrator systemu",
            };

            var resultDefaultAdmin = await userManager.CreateAsync(newAdmin, Configuration.GetConnectionString("AdminPassword"));
            await userManager.AddToRoleAsync(newAdmin, "administrator");
            if (resultDefaultAdmin.Succeeded) logger.LogInformation("Utworzono domyślnego administratora.");

            var newApplicationUser = new ApplicationUser
            {
                UserName = "kowalski@gmail.com",
                Email = "kowalski@gmail.com",
                FirstName = "Kowalski",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 2,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

            newApplicationUser = new ApplicationUser
            {
                UserName = "jan@gmail.com",
                Email = "jan@gmail.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 3,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

            newApplicationUser = new ApplicationUser
            {
                UserName = "brajanusz@gmail.com",
                Email = "brajanusz@gmail.com",
                FirstName = "Brajanusz",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 4,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

            newApplicationUser = new ApplicationUser
            {
                UserName = "Dzesika@gmail.com",
                Email = "Dzesika@gmail.com",
                FirstName = "Dżesika",
                LastName = "Kowalska",
                Note = "Przykładowy użytkownik",
                Number = 5,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

            newApplicationUser = new ApplicationUser
            {
                UserName = "Piter@gmail.com",
                Email = "Piter@gmail.com",
                FirstName = "Piter",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 6,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

            newApplicationUser = new ApplicationUser
            {
                UserName = "Dzordz@gmail.com",
                Email = "Dzordz@gmail.com",
                FirstName = "Dżordż",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 7,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            resultDefaultUser = await userManager.CreateAsync(newApplicationUser, Configuration.GetConnectionString("UserPassword"));
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");


            var departments = new Department[]
{
            new Department { Name = "Automatyki i Robotyki", Administrator = 2},
            new Department { Name = "Informatyki", Administrator = 3 },
            new Department { Name = "Materiałoznastwa", Administrator =  4 },
            new Department { Name = "Elektrotechniki", Administrator = 5 },
            new Department { Name = "Miernictwa", Administrator = 6 },
};
            foreach (Department c in departments)
            {
                context.Departments.Add(c);
            }
            context.SaveChanges();



            var buildings = new Building[]
            {
            new Building{Name="A",Address="NY, Manhatan 1a",DepartmentID=departments.Single( i => i.Name == "Automatyki i Robotyki").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="B",Address="NY, Manhatan 1b",DepartmentID=departments.Single( i => i.Name == "Automatyki i Robotyki").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="C",Address="NY, Manhatan 1c",DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="D",Address="NY, Manhatan 2a",DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="E",Address="NY, Manhatan 2b",DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="F",Address="NY, Manhatan 3",DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="G",Address="NY, Manhatan 4",DepartmentID=departments.Single( i => i.Name == "Elektrotechniki").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"},
            new Building{Name="H",Address="NY, Manhatan 5",DepartmentID=departments.Single( i => i.Name == "Miernictwa").DepartmentID, GPS_N="50.026829011836405", GPS_E="21.985347087936976"}
            };
            foreach (Building e in buildings)
            {
                context.Buildings.Add(e);
            }
            context.SaveChanges();


            var rooms = new Room[]
{
            new Room{Number=10, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=11, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=12, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=13, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=14, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=20, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=21, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=22, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{Number=23, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "A").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=14, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=21, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=22, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=23, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=31, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=32, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=33, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=34, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=41, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=42, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=43, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=44, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=51, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=52, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=53, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=54, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "B").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=14, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=21, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=22, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=23, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=31, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=32, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=33, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=34, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "C").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=14, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=20, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=21, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=22, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=23, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=25, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=26, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "D").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=14, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=15, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=16, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=20, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=21, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=22, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=23, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=25, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=26, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=31, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=32, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=33, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=34, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=35, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "E").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "F").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "F").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "F").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "F").BuildingID},
            new Room{ Number=14, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "F").BuildingID},
            new Room{ Number=10, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=11, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=12, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=13, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=21, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=22, Spots= 10, Type="Sala ćwiczeniowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=23, Spots= 10, Type="Sala laboratoryjna", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID},
            new Room{ Number=24, Spots= 10, Type="Sala wykładowa", Equipment="Wyposażenie...", BuildingID=buildings.Single( n => n.Name == "G").BuildingID}
            };

            foreach (Room e in rooms)
            {
                context.Rooms.Add(e);
            }
            context.SaveChanges();


        }
    }
}
