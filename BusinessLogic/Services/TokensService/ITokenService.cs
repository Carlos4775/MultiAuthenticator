using Data.Models;

namespace BusinessLogic.Services.TokensService
{
    public interface ITokenService
    {
        Token CreateToken(User user);
        string CreateRefreshToken();
    }
}
