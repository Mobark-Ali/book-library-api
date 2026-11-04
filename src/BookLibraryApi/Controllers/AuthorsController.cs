using BookLibraryApi.Data;
using BookLibraryApi.DTOs;
using BookLibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryContext _db;
    public AuthorsController(LibraryContext db) => _db = db;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        var authors = await _db.Authors.Include(a => a.Books)
            .Select(a => new AuthorDto(a.Id, a.Name, a.Country, a.Books.Count))
            .ToListAsync();
        return Ok(authors);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto dto)
    {
        var author = new Author { Name = dto.Name, Country = dto.Country };
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll),
            new AuthorDto(author.Id, author.Name, author.Country, 0));
    }
}
