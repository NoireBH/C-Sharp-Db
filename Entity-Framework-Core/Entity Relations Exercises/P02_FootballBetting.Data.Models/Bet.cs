﻿using P02_FootballBetting.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class Bet
{
    [Key]
    public int BetId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public Prediction Prediction {  get; set; }

    public DateTime DateTime { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId {  get; set; }

    public virtual User User { get; set; }

    [ForeignKey(nameof(Game))]
    [Required]
    public int GameId { get; set; }

    public virtual Game Game { get; set; }
}
