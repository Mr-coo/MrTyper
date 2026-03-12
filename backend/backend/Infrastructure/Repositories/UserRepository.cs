using backend.Domain.models;
using backend.Domain.Models;
using backend.Domain.Repositories;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> addUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> getByGithubId(string githubId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.githubId == githubId);
        }

        public async Task<User?> getByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.username == username);
        }

        public async Task storeRefreshToken(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
