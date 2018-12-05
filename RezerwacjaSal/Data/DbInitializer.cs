using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// dodano
using RezerwacjaSal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;

namespace RezerwacjaSal.Data
{
    // Inicjalizacja bazy danych jakimiś przykładowymi danymi 
    public class DbInitializer
    {

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

            var departments = new Department[]
{
            new Department { Name = "Automatyki i Robotyki", Administrator = 6},
            new Department { Name = "Informatyki", Administrator = 7 },
            new Department { Name = "Materiałoznastwa", Administrator =  8 },
            new Department { Name = "Elektrotechniki", Administrator =  11 },
            new Department { Name = "Miernictwa", Administrator = 12 },
};
            foreach (Department c in departments)
            {
                context.Departments.Add(c);
            }
            context.SaveChanges();

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
                UserName = "webapp0@outlook.com",
                Email = "webapp0@outlook.com",
                FirstName = "Admin",
                LastName = "Admin",
                Note = "Administrator systemu Bulbulator",
                Number = 0,
                EmailConfirmed = true,
                Employment = "Administrator systemu",
                DepartmentID = departments.Single(i => i.Name == "Automatyki i Robotyki").DepartmentID
            };

            var resultDefaultAdmin = await userManager.CreateAsync(newAdmin, "Asdasd.1");
            await userManager.AddToRoleAsync(newAdmin, "administrator");
            if (resultDefaultAdmin.Succeeded) logger.LogInformation("Utworzono domyślnego administratora.");

            var newApplicationUser = new ApplicationUser
            {
                UserName = "kowalski@gmail.com",
                Email = "kowalski@gmail.com",
                FirstName = "Kowalski",
                LastName = "Kowalski",
                Note = "Przykładowy użytkownik",
                Number = 1,
                EmailConfirmed = true,
                Employment = "Taki tam ...",
                DepartmentID = departments.Single(i => i.Name == "Automatyki i Robotyki").DepartmentID
            };

            var resultDefaultUser = await userManager.CreateAsync(newApplicationUser, "Asdasd.1");
            await userManager.AddToRoleAsync(newApplicationUser, "użytkownik");
            if (resultDefaultUser.Succeeded) logger.LogInformation("Utworzono domyślnego użytkownika.");

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
