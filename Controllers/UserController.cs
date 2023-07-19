using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetApi.Models

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly TodoContext _context;

    public UsersController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        return await _context.Users
            .Select(x => ItemToDTO(x))
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
    public async Task<IActionResult> PutUser(int id, UserDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var User = await _context.Users.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        User.Name = todoDTO.Name;
        User.IsComplete = todoDTO.IsComplete;

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

        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUser),
            new { id = User.Id },
            ItemToDTO(User));
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