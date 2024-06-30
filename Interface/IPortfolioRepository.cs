using AppUser.Models;

namespace AppUser.Interface
{
    public interface IPortfolioRepository
    {
        Task<ICollection<Stock>> GetUserPortfolio(AppUserT user);

        Task<Portfolio> CreateAsync(Portfolio portfolio);

        Task<Portfolio> DeleteAsync(AppUserT appUser ,string symbol);
    }
}
