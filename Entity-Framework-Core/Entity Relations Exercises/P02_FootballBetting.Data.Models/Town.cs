﻿using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    public Town()
    {
        Teams = new HashSet<Team>();
    }

    [Key]
    public int TownId { get; set; }

    [Required]
    [StringLength(ValidationConstants.TownNameMaxLength)]
    public string Name { get; set; }

    [Required]
    [ForeignKey(nameof(Country))]
    public int CountryId {  get; set; }

    public virtual Country Country { get; set; }

    public virtual ICollection<Team> Teams { get; set; }
}
