using Data;
using Data.Models;
using System.Security.Cryptography;
using UnitOfWork.UnitOfWork;

namespace BusinessLogic.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UnitOfWorksService _unitOfWork;

        public AuthenticationService(AppDbContext dbContext)
        {
            _unitOfWork = new UnitOfWorksService(dbContext);
        }

        public async Task PostAsync(User entity)
        {
            _unitOfWork.GenericRepository<User>().Post(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _unitOfWork.GenericRepository<User>().GetByAsync(x => x.Email != null && x.Email == email);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _unitOfWork.GenericRepository<User>().GetByAsync(x => x.Username != null && x.Username == username);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _unitOfWork.GenericRepository<User>().GetByAsync(x => x.RefreshToken != null && x.RefreshToken == refreshToken);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            string hashedEnteredPassword = HashPassword(actualPassword);
            return string.Equals(hashedEnteredPassword, hashedPassword);
        }

        public async Task<string> ValidateData(User user)
        {
            User? currentUser;

            if (string.IsNullOrEmpty(user.Username)) 
            {
                return "Username or email must be provided";
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return "Password must be provided";
            }

            if (user.Username.Contains('@'))
            {
                currentUser = await GetByEmailAsync(user.Username);
            }
            else
            {
                currentUser = await GetByUsernameAsync(user.Username);
            }

            if (currentUser == null)
            {
                return "No registered user for this email.";
            }

            if (string.IsNullOrEmpty(currentUser.Password))
            {
                return "User password is missing or invalid.";
            }

            bool passwordValid = VerifyPassword(user.Password, currentUser.Password);

            if (!passwordValid)
            {
                return "Invalid password";
            }

            return string.Empty;
        }
    }
}
