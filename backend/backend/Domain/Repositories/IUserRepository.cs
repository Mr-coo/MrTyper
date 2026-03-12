using backend.Domain.models;
using backend.Domain.Models;

namespace backend.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<User?> getByUsernameAsync(string username);
        Task<User?> getByGithubId(string githubId);

        Task storeRefreshToken(RefreshToken refreshToken);
    }
}
