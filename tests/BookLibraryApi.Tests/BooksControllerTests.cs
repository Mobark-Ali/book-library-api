using BookLibraryApi.Controllers;
using BookLibraryApi.Data;
using BookLibraryApi.DTOs;
using BookLibraryApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookLibraryApi.Tests;

public class BooksControllerTests
{
    private LibraryContext GetDb()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public async Task GetAll_ReturnsAllBooks()
    {
        using var db = GetDb();
        db.Authors.Add(new Author { Id = 1, Name = "A" });
        db.Books.AddRange(
            new Book { Id = 1, Title = "B1", AuthorId = 1, PublishedYear = 2000 },
            new Book { Id = 2, Title = "B2", AuthorId = 1, PublishedYear = 2001 });
        await db.SaveChangesAsync();

        var controller = new BooksController(db);
        var result = await controller.GetAll();
        var ok = result.Result as OkObjectResult;
        var list = ok!.Value as IEnumerable<BookDto>;

        list.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_WithInvalidAuthor_ReturnsBadRequest()
    {
        using var db = GetDb();
        var controller = new BooksController(db);

        var result = await controller.Create(new CreateBookDto("T", null, 2020, 999));

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_ExistingBook_RemovesIt()
    {
        using var db = GetDb();
        db.Authors.Add(new Author { Id = 1, Name = "A" });
        db.Books.Add(new Book { Id = 5, Title = "X", AuthorId = 1, PublishedYear = 2020 });
        await db.SaveChangesAsync();

        var controller = new BooksController(db);
        var result = await controller.Delete(5);

        result.Should().BeOfType<NoContentResult>();
        db.Books.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        using var db = GetDb();
        var controller = new BooksController(db);

        var result = await controller.GetById(42);

        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
