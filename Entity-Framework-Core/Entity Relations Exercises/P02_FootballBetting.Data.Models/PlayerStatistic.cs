using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class PlayerStatistic
{
    
    public int GameId {  get; set; }

    public int PlayerId {  get; set; }

    [Required]
    [MaxLength(ValidationConstants.ScoredGoalsMax)]
    public int ScoredGoals {  get; set; }

    [Required]
    [MaxLength(ValidationConstants.AssistsMax)]
    public int Assists { get; set; }

    public int MinutesPlayed {  get; set; }
}
