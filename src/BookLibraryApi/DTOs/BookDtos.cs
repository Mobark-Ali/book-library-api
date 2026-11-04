namespace BookLibraryApi.DTOs;

public record BookDto(int Id, string Title, string? ISBN, int PublishedYear, string AuthorName);
public record CreateBookDto(string Title, string? ISBN, int PublishedYear, int AuthorId);
public record UpdateBookDto(string Title, string? ISBN, int PublishedYear);
