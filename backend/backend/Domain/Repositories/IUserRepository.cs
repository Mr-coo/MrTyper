using backend.Domain.models;

namespace backend.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<User?> getByUsernameAsync(string username);
        Task<User?> getByGithubId(string githubId);
    }
}
