﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orders.Shared.Interfaces;

namespace Orders.Shared.Entities;

public class State :IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Estado")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    public int CountryId { get; set; }
    public Country? Country { get; set; }

    public ICollection<City>? Cities { get; set; }
    [Display(Name = "Ciudades")]
    public int CitiesNumber => Cities == null || Cities.Count == 0 ? 0 : Cities.Count;
}
