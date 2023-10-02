using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    [Key]
    public int PlayerId {  get; set; }

    [Required]
    [StringLength(ValidationConstants.PlayerNameMaxLength)]
    public string Name { get; set; }

    public int SquadNumber {  get; set; }

    public int TeamId { get; set; }

    [Required]
    public int PositionId {  get; set; }

    public bool IsInjured {  get; set; }
}
