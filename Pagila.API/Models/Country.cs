﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pagila.API.Models;

[Table("country")]
public partial class Country
{
    [Key]
    [Column("country_id")]
    public int CountryId { get; set; }

    [Required]
    [Column("country")]
    public string Country1 { get; set; }

    [Column("last_update")]
    public DateTime LastUpdate { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}