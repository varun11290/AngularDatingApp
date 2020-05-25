using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using DatingApp.API.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _auth;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepo auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDTO)
        {

            createUserDTO.UserName = createUserDTO.UserName.ToLower();
            if (await _auth.UserExist(createUserDTO.UserName))
                return BadRequest("User name alerady exist");

            var userToCreate = new User()
            {
                Name = createUserDTO.UserName
            };

            var createduser = await _auth.Register(userToCreate, createUserDTO.Password);
            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            var user = await _auth.Login(loginUserDTO.UserName, loginUserDTO.Password);
            if (user == null)
                return Unauthorized();

            //To generat a tocken for that first we have to create claims

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.Name)
            };
            //now we will create a key 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config
                                                .GetSection("AppSetting:Token").Value));

            //Now we will create credential with tis key    
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Now we will creat Token description

            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            //now we will creat token handler which will creat a token

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}