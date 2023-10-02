using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [StringLength(ValidationConstants.TeamNameMaxLength)]
        public string Name { get; set; }

        [StringLength(ValidationConstants.TeamLogoUrlMaxLength)]
        public string LogoUrl { get; set; }

        [Required]
        [StringLength(ValidationConstants.TeamInitialsMaxLength)]
        public string Initials { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        public int PrimaryKitColorId { get; set; }

        [Required]
        public int SecondaryKitColorId {  get; set; }

        public int TownId {  get; set; }
    }
}