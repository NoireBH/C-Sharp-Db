﻿using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    [Key]
    public int TownId { get; set; }

    [Required]
    [StringLength(ValidationConstants.TownNameMaxLength)]
    public string Name { get; set; }

    [Required]
    public int CountryId {  get; set; }
}
