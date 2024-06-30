using AppUser.Extensions;
using AppUser.Interface;
using AppUser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly UserManager<AppUserT> _userManager;

        public PortfolioController(
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository,
            UserManager<AppUserT> userManager)
        {
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Portfolio>))]
        public async Task<IActionResult> GetUserPortfolio()
        {
            // Check if the User is not null
            if (User == null)
            {
                return Unauthorized("User is not authenticated");
            }

            string username;
            try
            {
                // Get the username from the claims
                username = User.GetUsername();
            }
            catch (ArgumentNullException ex)
            {
                // Handle null user scenario
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Handle missing claim scenario
                return BadRequest(ex.Message);
            }

            // Find the user by username
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return NotFound("User not found");
            }

            // Get the user's portfolio
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            //Get Username
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);


            //Get Stock
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock Not Found");


            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) 
                return BadRequest("Cannot add same stock to portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500,"Could not create");
            }
            else
            {
                return Created();
            }

        }



        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            //Get Username from Token throw Authorize
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);


            //one or more
            var filterStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

            if (filterStock.Count() == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }

            return Ok();

        }


    }
}
