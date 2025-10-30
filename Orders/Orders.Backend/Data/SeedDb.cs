

using Microsoft.EntityFrameworkCore;

using Orders.Shared.Entities;

namespace Orders.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
	{
        _context = context;
    }

    public async Task SeedDbAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesFullAsync();
        await CheckCountriesAsync();
        await CheckCategoriesAsync();
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
            _context.Categories.Add(new Category { Name = "Calzado" });
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
}
