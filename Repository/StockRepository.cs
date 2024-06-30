using AppUser.Data;
using AppUser.Dto.comment;
using AppUser.Dto.stock;
using AppUser.Helpers;
using AppUser.Interface;
using AppUser.Mapper;
using AppUser.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppUser.Repository
{
    public class StockRepository:IStockRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public StockRepository(ApplicationDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<StockDto>> GetStocks(QueryObject query)
        {
            var stocks = _context.Stocks
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.AppUser)
                               .AsQueryable();

            //Filtration
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            //Filtration
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            //Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {

                if (query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending 
                        ? stocks.OrderByDescending(s => s.Symbol)
                        : stocks.OrderBy(s => s.Symbol);

                }
                
            }

           
            var skipNumber = (query.PageNumber - 1) * (query.PageSize);
            

            var result = stocks.Select(stock => new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,  
                Comments = stock.Comments.Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    Title = comment.Title,
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    CreatedAt = comment.AppUser.UserName,
                    StockId = comment.StockId
                }).ToList()
            }).Skip(skipNumber).Take(query.PageSize).ToList(); 

            return result;
        }

        public async Task<StockDto> GetStock(int id)
        {

            var stock = await _context.Stocks
                              .Include(x => x.Comments)
                              .ThenInclude(x => x.AppUser)
                              .FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null)
            {
                return null; 
            }

            var result = new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    Title = comment.Title,
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    CreatedAt = comment.AppUser.UserName,
                    StockId = comment.StockId
                }).ToList()
            };

            return result;
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateStock(Stock stock)
        {
           await _context.Stocks.AddAsync(stock);
           await Save();
           return stock;
        }

        public async Task<bool> Save()
        {
            var saved = await  _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Stock?> UpdateStock(int id, Stock stock)
        {

            var stocksExacting = await _context.Stocks.FirstOrDefaultAsync( x => x.Id == id);

            if (stocksExacting == null)
                return null;

            stocksExacting.Purchase = stock.Purchase;
            stocksExacting.MarketCap = stock.MarketCap;
            stocksExacting.Industry = stock.Industry;
            stocksExacting.Symbol = stock.Symbol;
            stocksExacting.LastDiv = stock.LastDiv;
            stocksExacting.CompanyName = stock.CompanyName;


            await Save();

            return stock;
        }

        public async Task<Stock?> DeleteStock(int id)
        {
            var stocks = await _context.Stocks.FindAsync(id);

            if (stocks == null)
                return null;
            
           _context.Stocks.Remove(stocks);

           await Save();

           return stocks;
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
            return stock;
        }
    }
    }

