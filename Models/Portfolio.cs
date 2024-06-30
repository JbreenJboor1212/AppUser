using System.ComponentModel.DataAnnotations.Schema;

namespace AppUser.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        //Stock
        public int StockId { get; set; }
        public Stock Stock { get; set; } = null;

        //Stock
        public string AppUserId { get; set; }
        public AppUserT AppUserT { get; set; } = null;
    }
}
