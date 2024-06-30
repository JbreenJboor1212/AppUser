using AppUser.Dto.stock;
using AppUser.Helpers;
using AppUser.Interface;
using AppUser.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public StockController(
            IStockRepository stockRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _stockRepository = stockRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Stock>))]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = _mapper.Map<List<StockDto>>(await _stockRepository.GetStocks(query));

            return Ok(stocks);
        }


        //Validation => :int
        [HttpGet("{stockId:int}")]
        [ProducesResponseType(200, Type = typeof(Stock))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetStock(int stockId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _stockRepository.StockExist(stockId) == false)
                return NotFound();

            var stock = _mapper.Map<StockDto>(await _stockRepository.GetStock(stockId));


            return Ok(stock);
        }



        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stockCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (stockCreate == null)
            {
                return BadRequest(ModelState);
            }


            var stockMap = await _stockRepository.CreateStock(_mapper.Map<Stock>(stockCreate));

            if (stockMap == null) // Assuming CreateStock should be async
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
           
                return StatusCode(201, _mapper.Map<StockDto>(stockMap));

        }

        //Validation => :int
        [HttpPut("{stockId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStock(int stockId, [FromBody] UpdateStockDto stockUpdate)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (stockUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepository.StockExist(stockId))
            {
                return NotFound();
            }

            var stockMap = _mapper.Map<Stock>(stockUpdate);          
            

            if (await _stockRepository.UpdateStock(stockId, stockMap) == null)
            {
                ModelState.AddModelError("", "Something went wrong updating stock");
                return StatusCode(500, ModelState);
            }

            return Ok(_mapper.Map<StockDto>(stockMap));
        }

        //Validation => :int
        [HttpDelete("{stockId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStock(int stockId)
        {

            //Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //404
            if (await _stockRepository.StockExist(stockId) == false)
                return NotFound();

            var stock  = _stockRepository.GetStock(stockId);

            //204
            if (await _stockRepository.DeleteStock(stockId) == null)
            {
                ModelState.AddModelError("", "Something went wrong delete stock");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
