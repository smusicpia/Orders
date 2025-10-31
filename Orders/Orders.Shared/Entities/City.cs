﻿using System.ComponentModel.DataAnnotations;

using Orders.Shared.Interfaces;

namespace Orders.Shared.Entities;

public class City : IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Ciudad")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    public int StateId { get; set; }
    public State? State { get; set; } = null!;

    public ICollection<City>? Cities { get; set; }

    public int CitiesNumber => Cities == null || Cities.Count == 0 ? 0 : Cities.Count;
}
