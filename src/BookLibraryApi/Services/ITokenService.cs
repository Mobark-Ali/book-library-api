using BookLibraryApi.Models;

namespace BookLibraryApi.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
