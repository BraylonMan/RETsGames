using System;
using System.ComponentModel.DataAnnotations;

namespace RETsGames.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public int Quantity { get; set; } = 1;

        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        public string Buyer { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string BuyerEmail { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string CardLast4 { get; set; } = string.Empty;
    }
}
