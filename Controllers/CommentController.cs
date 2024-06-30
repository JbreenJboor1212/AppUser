using AppUser.Dto.comment;
using AppUser.Dto.stock;
using AppUser.Extensions;
using AppUser.Interface;
using AppUser.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace AppUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUserT> _userManager;
        private readonly IMapper _mapper;

        public CommentController(
            ICommentRepository commentRepository,
            IStockRepository stockRepository,
            UserManager<AppUserT> userManager,
            IMapper mapper
            )
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDto>))]
        public async Task<IActionResult> GetAll()
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = _mapper.Map<List<CommentDto>>(await _commentRepository.GetAllAsync());          

            return Ok(comments);
        }

        //Validation => :int
        [HttpGet("{commentId:int}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetComment(int commentId)
        {

            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _commentRepository.CommentExistAsync(commentId) == false)
                return NotFound();

            var comment = _mapper.Map<CommentDto>(await _commentRepository.GetCommentAsync(commentId));
            

            return Ok(comment);
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateComment([FromQuery] int stockId, [FromBody] CreateCommentDto commentCreate)
        {

            //400
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //204
            if (!await _stockRepository.StockExist(stockId)) return BadRequest("Stock Does Not Found");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            //400
            if (commentCreate == null) return BadRequest(ModelState);

            var convertComment = _mapper.Map<Comment>(commentCreate);

            convertComment.AppUserId = appUser.Id;

            var comment = await _commentRepository.CreateCommentAsync(stockId, convertComment);

            if (comment == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var CommentDtoT = _mapper.Map<CommentDto>(comment);

            CommentDtoT.CreatedAt = appUser.UserName;

            return StatusCode(201, CommentDtoT);
        }


        //Validation => :int
        [HttpPut]
        [Route("{commentId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentDto commentUpdate)
        {

            //400
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (commentUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _commentRepository.CommentExistAsync(commentId))
            {
                return NotFound();
            }


            var commentMap = _mapper.Map<Comment>(commentUpdate);


            if (await _commentRepository.UpdateCommentAsync(commentId, commentMap) == null)
            {
                ModelState.AddModelError("", "Something went wrong updating comment");
                return StatusCode(500, ModelState);
            }

            return Ok(commentUpdate);
        }


        //Validation => :int
        [HttpDelete]
        [Route("{commentId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentExist = await _commentRepository.CommentExistAsync(commentId);

            if (!commentExist)
            {
                return NotFound("Comment Does Not Exist");
            }


            if (await _commentRepository.DeleteCommentAsync(commentId) == null)
            {
                ModelState.AddModelError("", "Something went wrong deleting comment");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleting Successfully");
        }


    }
}
