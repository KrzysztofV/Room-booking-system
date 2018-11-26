using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// dodano
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Data
{
    // Inicjalizacja bazy danych jakimiś przykładowymi danymi 
    public class DbInitializer
    {
        public static void Initialize(RezerwacjaSalContext context)
        {
            context.Database.EnsureCreated();

            // Sprawdź czy są jacyś pracownicy
            if (context.AppUsers.Any())
            {
                return;   // są więc nie nadpisuj Bazy danych 
            }

            //var people = new ApplicationUser[]
            //{
            //new ApplicationUser{Number=1,FirstName="Putin",  LastName="Władimir",        Employee=false,Email="Put@in.ru",Phone="123456789",Note="Gość z zagranicy :P"},
            //new ApplicationUser{Number=2,FirstName="Donald", LastName="Trump",           Employee=false,Email="Trump@manhatan.com",Phone="123456789",Note="Gość z zagranicy :P"},
            //new ApplicationUser{Number=3,FirstName="Andrzej",LastName="Duda",            Employee=false,Email="Buziaczek@interia.pl",Phone="123456789",Note=""},
            //new ApplicationUser{Number=4,FirstName="Arnold", LastName="Schwarzenegger",  Employee=false,Email="Arni@hoolybool.com",Phone="123456789",Note="Gość z zagranicy :P"},
            //new ApplicationUser{Number=5,FirstName="Kim Dzong",LastName="Un",            Employee=false,Email="",Phone="123456789",Note="Gość z zagranicy :P"},
            //new ApplicationUser{Number=6,FirstName="Jan",    LastName="Kowalski",        Employee=true,Email="JK275@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=7,FirstName="Brajanusz",LastName="Kowalski",      Employee=true,Email="BrajaneX@wp.pl",Phone="123456789",Note=""},
            //new ApplicationUser{Number=8,FirstName="Brajan", LastName="Kowalski",        Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=9,FirstName="Mr.Potato",LastName="French Fries",  Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=10,FirstName="Antonio",LastName="Banderas",       Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=11,FirstName="Tomasz",LastName="Kowalski",        Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=12,FirstName="Mateusz",LastName="Kowalski",       Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=13,FirstName="Jakub", LastName="Miszcz",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=14,FirstName="Wojciech",LastName="Harasymowicz",  Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=15,FirstName="Rafał", LastName="Ileczko",         Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=16,FirstName="Krzysztof",LastName="Cisło",        Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=17,FirstName="Damian",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=18,FirstName="X",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=19,FirstName="X",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=20,FirstName="X",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=21,FirstName="X",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=22,FirstName="X",LastName="Drozda",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=23,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=24,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=25,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=26,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=27,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=28,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=29,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=30,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=31,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=32,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=33,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=34,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=35,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=36,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=37,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=38,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=39,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=40,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=41,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=42,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=43,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=44,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=45,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=46,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=47,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=48,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=49,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=50,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=51,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=52,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""},
            //new ApplicationUser{Number=53,FirstName="X",LastName="Y",          Employee=true,Email="123@gmail.com",Phone="123456789",Note=""}
            //};

            //foreach (ApplicationUser s in people)
            //{
            //    context.AppUsers.Add(s);
            //}
            //context.SaveChanges();

            var departments = new Department[]
            {
            new Department { Name = "Automatyki i Robotyki", Administrator = 6, Manager = 16},
            new Department { Name = "Informatyki", Administrator = 7, Manager = 17 },
            new Department { Name = "Materiałoznastwa", Administrator =  8, Manager =  13 },
            new Department { Name = "Elektrotechniki", Administrator =  11, Manager =  15 },
            new Department { Name = "Miernictwa", Administrator = 12, Manager =  14 },
            };
            foreach (Department c in departments)
            {
                context.Departments.Add(c);
            }
            context.SaveChanges();

            //var employments = new Employment[]
            //{
            //    // nie podaję EmploymentID ponieważ w class Employment, EmploymentID jest on automatycznie nadawany przez Entity Framework
            //new Employment{Id=people.Single( i => i.Number == 6).Id, DepartmentID=departments.Single( i => i.Name == "Automatyki i Robotyki").DepartmentID,  Position="Administrator",       },
            //new Employment{Id=people.Single( i => i.Number == 7).Id, DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID,  Position="Administrator",       },
            //new Employment{Id=people.Single( i => i.Number == 8).Id, DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID,  Position="Administrator",       },
            //new Employment{Id=people.Single( i => i.Number == 9).Id,  DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID,  Position="Adiunk",              },
            //new Employment{Id=people.Single( i => i.Number == 10).Id, DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID, Position="Adiunk",              },
            //new Employment{Id=people.Single( i => i.Number == 11).Id, DepartmentID=departments.Single( i => i.Name == "Elektrotechniki").DepartmentID, Position="Administrator",       },
            //new Employment{Id=people.Single( i => i.Number == 12).Id, DepartmentID=departments.Single( i => i.Name == "Miernictwa").DepartmentID, Position="Administrator",       },
            //new Employment{Id=people.Single( i => i.Number == 13).Id, DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID, Position="Kierownik katedry",   },
            //new Employment{Id=people.Single( i => i.Number == 14).Id, DepartmentID=departments.Single( i => i.Name == "Miernictwa").DepartmentID, Position="Kierownik katedry",   },
            //new Employment{Id=people.Single( i => i.Number == 15).Id, DepartmentID=departments.Single( i => i.Name == "Elektrotechniki").DepartmentID, Position="Kierownik katedry",   },
            //new Employment{Id=people.Single( i => i.Number == 16).Id, DepartmentID=departments.Single( i => i.Name == "Automatyki i Robotyki").DepartmentID, Position="Kierownik katedry",   },
            //new Employment{Id=people.Single( i => i.Number == 16).Id, DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID, Position="Takie tam",   },
            //new Employment{Id=people.Single( i => i.Number == 17).Id, DepartmentID=departments.Single( i => i.Name == "Materiałoznastwa").DepartmentID, Position="Takie tam",},
            //new Employment{Id=people.Single( i => i.Number == 17).Id, DepartmentID=departments.Single( i => i.Name == "Informatyki").DepartmentID, Position="Kierownik katedry"}

            //};
            //foreach (Employment e in employments)
            //{
            //    context.Employments.Add(e);
            //}
            //context.SaveChanges();


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

            //var reservations = new Reservation[]
            //{
            //new Reservation{RoomID=rooms.Single(r=>r.Number==10 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID, Id=people.Single( i => i.Number == 1).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==11 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID, Id=people.Single( i => i.Number == 2).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==12 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 3).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==13 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 4).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==14 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 5).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==20 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 6).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==21 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 7).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==22 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 8).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==23 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 9).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==24 && r.BuildingID==buildings.Single( n => n.Name == "A").BuildingID).RoomID , Id=people.Single( i => i.Number == 10).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu" },
            //new Reservation{RoomID=rooms.Single(r=>r.Number==10 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 11).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==11 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 12).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu" },
            //new Reservation{RoomID=rooms.Single(r=>r.Number==12 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 13).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu" },
            //new Reservation{RoomID=rooms.Single(r=>r.Number==13 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 14).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==14 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 16).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==21 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 17).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==22 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 17).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==23 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 16).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==24 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 16).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00), Date= new DateTime(2018,07,14,00,00,00) , Note="Łubudu"},
            //new Reservation{RoomID=rooms.Single(r=>r.Number==31 && r.BuildingID==buildings.Single( n => n.Name == "B").BuildingID).RoomID , Id=people.Single( i => i.Number == 14).Id, StartTime=new DateTime(2018,07,14,10,00,00), EndTime=new DateTime(2018,07,14,12,00,00) , Date= new DateTime(2018,07,14,00,00,00), Note="Łubudu"}
            //};
            //foreach (Reservation e in reservations)
            //{
            //    context.Reservations.Add(e);
            //}
            //context.SaveChanges();

            //var messages = new Message[]
            //{
            //    new Message{ Email = "123@123.com", Name = "Imie1 Nazwisko1", MessageSubject="Temat", MessageContent="Treść",IP="31.120.152.175", Date = new DateTime(2018,07,14,10,00,00), Id = people.Single( r => r.Number == 1).Id  },
            //    new Message{ Email = "234@234.com", Name = "Imie2 Nazwisko2", MessageSubject="Temat", MessageContent="Treść",IP="31.120.152.176", Date = new DateTime(2018,07,14,10,00,00), Id = people.Single( r => r.Number == 2).Id  },
            //    new Message{ Email = "345@345.com", Name = "Imie3 Nazwisko3", MessageSubject="Temat", MessageContent="Treść",IP="31.120.152.177", Date = new DateTime(2018,07,14,10,00,00), Id = people.Single( r => r.Number == 3).Id  }

            //};
            //foreach (Message e in messages)
            //{
            //    context.Messages.Add(e);
            //}
            //context.SaveChanges();
        }
    }
}
