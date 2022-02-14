using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HorseWebApi.Entities;
using Microsoft.Extensions.Options;
using HorseWebApi.Settings;
using HorseWebApi.Repositories;
using HorseWebApi.Infrastructure;

namespace HorseWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UsersRepository usersrepository;
        private SettingsJWT jwtOptions;

        public AccountController(
            IOptions<SettingsJWT> jwtOptions, 
            IGenericRepository<User> usersrepository)
        {
            this.usersrepository = usersrepository as UsersRepository ??
                 throw new ArgumentNullException(nameof(usersrepository));

            this.jwtOptions = jwtOptions.Value;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(jwtOptions.ExpiryTime)),
                    signingCredentials: new SigningCredentials(jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(string username, string password)
        {
            User user = await usersrepository.FindAsync(x => x.Login == username);
            if (user is null)
            {
                var newUser = new User { Name = username, Login = username, Role = "Customer" };
                newUser.Salt = PasswordHandler.GetSecureByteArray(32);
                newUser.Hash = PasswordHandler.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), newUser.Salt);

                await usersrepository.AddAsync(newUser);

                return Ok();
            }
            else
                return BadRequest();
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) 
                return null;
            
            User user = await usersrepository.FindAsync(x => x.Login != "" && x.Login != null && x.Login == username);
            if (user != null)
            {
                var hash = PasswordHandler.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), user.Salt);

                if (!PasswordHandler.CompareByteArrays(user.Hash, hash)) return null;

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
