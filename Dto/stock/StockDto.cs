using AppUser.Dto.comment;
using AppUser.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppUser.Dto.stock
{
    public class StockDto
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; } // long > int

        public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
