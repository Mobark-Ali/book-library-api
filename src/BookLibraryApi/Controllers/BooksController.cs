using BookLibraryApi.Data;
using BookLibraryApi.DTOs;
using BookLibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _db;
    public BooksController(LibraryContext db) => _db = db;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
    {
        var books = await _db.Books.Include(b => b.Author)
            .Select(b => new BookDto(b.Id, b.Title, b.ISBN, b.PublishedYear, b.Author!.Name))
            .ToListAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var b = await _db.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        if (b is null) return NotFound();
        return Ok(new BookDto(b.Id, b.Title, b.ISBN, b.PublishedYear, b.Author!.Name));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
    {
        if (!await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId))
            return BadRequest("Author not found.");

        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            PublishedYear = dto.PublishedYear,
            AuthorId = dto.AuthorId
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = book.Id },
            new BookDto(book.Id, book.Title, book.ISBN, book.PublishedYear, string.Empty));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateBookDto dto)
    {
        var book = await _db.Books.FindAsync(id);
        if (book is null) return NotFound();

        book.Title = dto.Title; book.ISBN = dto.ISBN; book.PublishedYear = dto.PublishedYear;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book is null) return NotFound();
        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
