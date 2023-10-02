using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    
    public int GameId {  get; set; }

    [Required]
    public int HomeTeamId {  get; set; }

    [Required]
    public int AwayTeamId { get; set; }

    [Required]

    public int AwayTeamGoals {  get; set; }

    public DateTime DateTime { get; set; }

    public decimal HomeTeamBetRate { get; set; }

    public decimal AwayTeamBetRate { get; set; }

    public decimal DrawBetRate { get; set; }

    [Required]
    [MaxLength(10)]
    public string Result {  get; set; }

}
