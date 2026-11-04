using BookLibraryApi.Data;
using BookLibraryApi.DTOs;
using BookLibraryApi.Models;
using BookLibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LibraryContext _db;
    private readonly ITokenService _tokenService;

    public AuthController(LibraryContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username already exists.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Admin"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _tokenService.CreateToken(user);
        return Ok(new AuthResponseDto(token, user.Username, user.Role));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _tokenService.CreateToken(user);
        return Ok(new AuthResponseDto(token, user.Username, user.Role));
    }
}
