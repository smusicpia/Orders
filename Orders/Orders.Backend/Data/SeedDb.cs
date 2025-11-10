

using Microsoft.EntityFrameworkCore;

using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
	{
        _context = context;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedDbAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesFullAsync();
        await CheckCountriesAsync();
        await CheckCategoriesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
    }

    private async Task CheckCountriesFullAsync()
    {
        if(!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }

    private async Task CheckCategoriesAsync()
    {
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category { Name = "Apple" });
            _context.Categories.Add(new Category { Name = "Autos" });
            _context.Categories.Add(new Category { Name = "Belleza" });
            _context.Categories.Add(new Category { Name = "Calzado" });
            _context.Categories.Add(new Category { Name = "Comida" });
            _context.Categories.Add(new Category { Name = "Cosmeticos" });
            _context.Categories.Add(new Category { Name = "Deportes" });
            _context.Categories.Add(new Category { Name = "Erótica" });
            _context.Categories.Add(new Category { Name = "Ferreteria" });
            _context.Categories.Add(new Category { Name = "Gamer" });
            _context.Categories.Add(new Category { Name = "Hogar" });
            _context.Categories.Add(new Category { Name = "Jardín" });
            _context.Categories.Add(new Category { Name = "Jugetes" });
            _context.Categories.Add(new Category { Name = "Lenceria" });
            _context.Categories.Add(new Category { Name = "Mascotas" });
            _context.Categories.Add(new Category { Name = "Nutrición" });
            _context.Categories.Add(new Category { Name = "Ropa" });
            _context.Categories.Add(new Category { Name = "Tecnología" });

            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckCountriesAsync()
    {
       if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country {
                Name = "Colombia",
                States = [
                    new State()
                    {
                        Name = "Antioquia",
                        Cities = [
                            new City() { Name = "Medellín" },
                            new City() { Name = "Itagüí" },
                            new City() { Name = "Envigado" },
                            new City() { Name = "Bello" },
                            new City() { Name = "Rionegro" }
                        ]
                    },
                    new State()
                    {
                        Name = "Bogotá",
                        Cities = [
                            new City() { Name = "Usaquen" },
                            new City() { Name = "Champinero" },
                            new City() { Name = "Santa fe" },
                            new City() { Name = "Useme" },
                            new City() { Name = "Bosa" }
                        ]
                    },
                    ]
            });
            _context.Countries.Add(new Country {
                Name = "Estados Unidos",
                States = [
                    new State()
                    {
                        Name = "California",
                        Cities = [
                            new City() { Name = "Los Angeles" },
                            new City() { Name = "San Diego" },
                            new City() { Name = "San Francisco" },
                            new City() { Name = "San Jose" },
                            new City() { Name = "Sacramento" }
                        ]
                    },
                    new State()
                    {
                        Name = "Florida",
                        Cities = [
                            new City() { Name = "Fort Lauderdale" },
                            new City() { Name = "Hialeah" },
                            new City() { Name = "Jacksonville" },
                            new City() { Name = "Kew West" },
                            new City() { Name = "Miami" },
                            new City() { Name = "Orlando" },
                            new City() { Name = "Tampa" },
                            new City() { Name = "Tallahassee" }
                        ]
                    },
                    new State()
                    {
                        Name = "Texas",
                        Cities = [
                            new City() { Name = "Austin" },
                            new City() { Name = "Dallas" },
                            new City() { Name = "El Paso" },
                            new City() { Name = "Houston" },
                            new City() { Name = "San Antonio" }
                        ]
                    },
                ]
            });
            _context.Countries.Add(new Country { Name = "Bolivia" });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
            city ??= await _context.Cities.FirstOrDefaultAsync();

            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Address = address,
                Document = document,
                City = city,
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

            var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            await _usersUnitOfWork.ConfirmEmailAsync(user, token);
        }

        return user;
    }

}
