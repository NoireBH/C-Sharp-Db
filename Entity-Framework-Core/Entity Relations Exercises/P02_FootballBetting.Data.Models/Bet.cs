using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public string Prediction {  get; set; }

    public DateTime DateTime { get; set; }

    [Required]
    public int UserId {  get; set; }

    [Required]
    public int GameId { get; set; }
}
