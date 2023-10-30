﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Common;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public PurchaseType Type { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.ProductKeyRegex)]
        public string ProductKey { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }

        [Required]
        public Card Card { get; set; }

        [Required]
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        [Required]
        public Game Game { get; set; }
    }
}
