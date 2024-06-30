using AppUser.Dto.Account;
using AppUser.Interface;
using AppUser.Models;
using AppUser.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AppUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUserT> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUserT> _signInManager;

        public AccountController(
            UserManager<AppUserT> userManager ,
            ITokenService tokenService ,
            SignInManager<AppUserT> signInManager
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            //Validation
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Check User
            var user = await _userManager.Users.FirstOrDefaultAsync(
                x => x.UserName == loginDto.Username.ToLower()
                );

            if (user == null) return Unauthorized("Invalid Username");

            //Check Password
            var result = await _signInManager.CheckPasswordSignInAsync(
                user,loginDto.Password,false
                );

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto )
        {
            try
            {
                //Validation
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUserT
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createUser = await _userManager
                    .CreateAsync(appUser, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                           new NewUserDto
                           { 
                               Username = appUser.UserName,
                               Email = appUser.Email,
                               Token = _tokenService.CreateToken(appUser)
                           }
                            );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex);
            }
        }

    }
}
