using System.ComponentModel.DataAnnotations.Schema;

namespace AppUser.Models
{
    [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty; // Avoid null

        public string CompanyName { get; set; } = string.Empty;// Avoid null

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; } // long > int

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    }
}
