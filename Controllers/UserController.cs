using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetApi.Models;
using DotNetApi.Infrastructure;
using TodoApi.Controllers;
using BCrypt.Net;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DataDbContext _context;

    public UsersController(DataDbContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        return await _context.Users
            .Select(x => UserToDTO(x))
            .ToListAsync();
    }

    // GET: api/Users/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUsers(int id)
    {
        var User = await _context.Users.FindAsync(id);

        if (User == null)
        {
            return NotFound();
        }

        return UserToDTO(User);
    }
    // </snippet_GetByID>

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
    {
        if (id != userDTO.Id)
        {
            return BadRequest();
        }

        var User = await _context.Users.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        User.Username = userDTO.Username;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
    {
        Methods methods = new Methods();
        var User = new User
        {
            Id = userDTO.Id,
            IdOfOrganization = userDTO.IdOfOrganization,
            Firstname = userDTO.Firstname,
            Username = userDTO.Username,
            Surname = userDTO.Surname,
            Email = userDTO.Email,
            Status = userDTO.Status

        };
        userDTO.Login = methods.TranslitName(userDTO.Firstname, userDTO.Username, userDTO.Surname);
        userDTO.Password = BCrypt.Net.BCrypt.HashPassword(methods.GetPass());
        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUsers),
            new { id = User.Id },
            UserToDTO(User));
    }
    // </snippet_Create>

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var User = await _context.Users.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        _context.Users.Remove(User);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    private static UserDTO UserToDTO(User User) =>
       new UserDTO
       {
           Id = User.Id,
           IdOfOrganization = User.IdOfOrganization,
           Firstname = User.Firstname,
           Username = User.Username,
           Surname = User.Surname,
           Email = User.Email,
           Status = User.Status
          
       };
}