using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/v1/accounts")]
public class UserAccountsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<UserAccountsController> _logger;
    private readonly IJwtService _jwtService;

    public UserAccountsController(ApiContext context, ILogger<UserAccountsController> logger, IJwtService jwtService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserDto>> CreateUserAccount(CreateUserAccountDto createUserAccountDto)
    {
        if (await UserAccountExists(createUserAccountDto.Username))
        {
            _logger.LogError("A user with username {} already exists.", createUserAccountDto.Username.ToLower());
            return BadRequest("Username is not available.");
        }
        
        using var hmac = new HMACSHA512();
        
        var user = new User
        {
            Username = createUserAccountDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserAccountDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new UserDto
        {
            Username = user.Username,
            JwToken = _jwtService.CreateJwt(user)
        });
    }
    
    [HttpPost]
    [Route("login")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserDto>> LoginUser(UserLoginDto userLoginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userLoginDto.Username);

        if (user == null) return Unauthorized("Username is invalid");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password is invalid");
        }

        return Ok(new UserDto
        {
            Username = user.Username,
            JwToken = _jwtService.CreateJwt(user)
        });
    }

    private async Task<bool> UserAccountExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
    }
}