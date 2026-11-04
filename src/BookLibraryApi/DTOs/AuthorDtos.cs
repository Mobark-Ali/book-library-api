namespace BookLibraryApi.DTOs;

public record AuthorDto(int Id, string Name, string? Country, int BookCount);
public record CreateAuthorDto(string Name, string? Country);
