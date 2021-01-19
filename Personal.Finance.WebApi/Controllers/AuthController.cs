using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Personal.Finance.Domain.Dtos;
using Personal.Finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Personal.Finance.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthController(IConfiguration config, ILogger<AuthController> logger, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _config = config;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register", Name = "register")]
        public async Task<ActionResult<UserRegisterDto>> Register([FromBody] UserRegisterDto dto)
        {
            if (await UserExists(dto.UserName).ConfigureAwait(false)) return StatusCode(500, "Take another user name");

            var userToCreate = UserRegisterDto.MapToEntity(dto);
            userToCreate.Created = DateTime.Now;
            userToCreate.UserName = userToCreate.UserName.ToLower(CultureInfo.InvariantCulture);


            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new Role {Name = "User"});
                await _roleManager.CreateAsync(new Role {Name = "Administrator"});
            }


            var result = await _userManager.CreateAsync(userToCreate, dto.Password).ConfigureAwait(false);
            await _userManager.AddToRoleAsync(userToCreate, "User").ConfigureAwait(false);
            // await _userManager.AddToRoleAsync(userToCreate, "Administrator").ConfigureAwait(false);

            return !result.Succeeded ? (ActionResult) BadRequest(result.Errors) : Ok(userToCreate);
        }

        [HttpPost("login", Name = "login")]
        public async Task<ActionResult<AuthUserDto>> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == dto.Username.ToLower(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);

            if (user == null) return StatusCode(401, "Not authorized");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false).ConfigureAwait(false);

            return !result.Succeeded
                ? StatusCode(401, "Check your user name or password")
                : StatusCode(200, new AuthUserDto {Token = GenerateJwtToken(user).Result, User = dto});
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}