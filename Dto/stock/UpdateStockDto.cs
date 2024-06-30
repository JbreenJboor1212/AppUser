using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppUser.Dto.stock
{
    public class UpdateStockDto
    {   //Validation
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 over characters")]
        public string Symbol { get; set; } = string.Empty;

        //Validation
        [Required]
        [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 over characters")]
        public string CompanyName { get; set; } = string.Empty;

        //Validation
        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        //Validation
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        //Validation
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
        public string Industry { get; set; } = string.Empty;

        //Validation
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}
