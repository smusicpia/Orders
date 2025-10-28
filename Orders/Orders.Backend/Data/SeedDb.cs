﻿
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
        await CheckCountriesAsync();
        await CheckCategoriesAsync();
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
            _context.Countries.Add(new Country { Name = "México" });
            _context.Countries.Add(new Country { Name = "Colombia" });
            _context.Countries.Add(new Country { Name = "Estados Unidos" });
            _context.Countries.Add(new Country { Name = "Bolivia" });
            await _context.SaveChangesAsync();
        }
    }
}
