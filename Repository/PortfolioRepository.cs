﻿ using AppUser.Data;
using AppUser.Interface;
using AppUser.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AppUser.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);

            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(AppUserT appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios
                .FirstOrDefaultAsync(s => s.AppUserId == appUser.Id
                && s.Stock.Symbol.ToLower() == symbol.ToLower());

            if (portfolioModel == null) 
            {
                return null;
            }

            _context.Portfolios.Remove(portfolioModel);

            await _context.SaveChangesAsync();

            return portfolioModel;
        }

        public async Task<ICollection<Stock>> GetUserPortfolio(AppUserT user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
                .Select(stock =>
                    new Stock
                    {
                        Id = stock.Stock.Id,
                        Symbol = stock.Stock.Symbol,
                        CompanyName = stock.Stock.CompanyName,
                        Purchase = stock.Stock.Purchase,
                        Industry = stock.Stock.Industry,
                        MarketCap = stock.Stock.MarketCap,
                        LastDiv = stock.Stock.LastDiv
                    }
                ).ToListAsync();
        }
    }
}
