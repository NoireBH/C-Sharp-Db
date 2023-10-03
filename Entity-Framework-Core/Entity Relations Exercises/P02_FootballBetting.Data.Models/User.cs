using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models;

public class User
{
    public User()
    {
        Bets = new HashSet<Bet>();
    }

    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.UserNameMaxLength)]
    public string Username { get; set; }

    [Required]
    [MaxLength(ValidationConstants.PasswordMaxLength)]
    public string Password { get; set; }

    [Required]
    [MaxLength(ValidationConstants.EmailMaxLength)]
    public string Email { get; set; }

    public string Name { get; set; }

    [Required]
    public decimal Balance { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }
}
