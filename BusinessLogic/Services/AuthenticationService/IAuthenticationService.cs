using Data.Models;

namespace BusinessLogic.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task PostAsync(User entity);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<string> ValidateData(User user);
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
    }
}
