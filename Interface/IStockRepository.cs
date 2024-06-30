using AppUser.Dto.stock;
using AppUser.Helpers;
using AppUser.Models;

namespace AppUser.Interface
{
    public interface IStockRepository
    {
        Task<ICollection<StockDto>> GetStocks(QueryObject query);

        Task<StockDto> GetStock(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);
         
        Task<bool> StockExist(int id);

        Task<Stock> CreateStock(Stock stock);

        Task<Stock?> UpdateStock(int id , Stock stock);

        Task<Stock?> DeleteStock(int id);

        Task<bool> Save();
    }
}
