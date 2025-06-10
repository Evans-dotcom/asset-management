using Asset_management.models;

namespace Asset_management.services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<List<User>> GetAllUsersAsync();

    }
}