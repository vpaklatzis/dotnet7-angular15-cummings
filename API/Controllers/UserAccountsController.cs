using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/v1/accounts")]
public class UserAccountsController
{
    private readonly APIContext _context;
    private readonly ILogger<UserAccountsController> _logger;

    public UserAccountsController(APIContext context, ILogger<UserAccountsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<User>> CreateUserAccount(CreateUserAccountDto createUserAccountDto)
    {
        if (await UserAccountExists(createUserAccountDto.UserName))
            return BadRequest("Username is not available");
        
        using var hmac = new HMACSHA512();
        
        var user = new User
        {
            UserName = createUserAccountDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserAccountDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private async Task<bool> UserAccountExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}