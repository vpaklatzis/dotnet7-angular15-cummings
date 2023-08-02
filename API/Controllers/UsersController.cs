using System.Net;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/users")]
public class UsersController : ControllerBase {

    private readonly ApiContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ApiContext context, ILogger<UsersController> logger) {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [AllowAnonymous]
    [HttpGet(Name = "Get Users")]
    [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
        var users = await _context.Users.ToListAsync();

        return Ok(users);
    }
    
    [HttpGet("{id}", Name = "Get User By Id")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<User>> GetUserById(string id) {
        var user = await _context.Users.FirstOrDefaultAsync();

        if (user == null) {
            _logger.LogError("User with id: {id} not found.", id);
            return NotFound();
        }

        return Ok(user);
    }

    
}