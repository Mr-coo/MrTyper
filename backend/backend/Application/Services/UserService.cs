using backend.Application.Dtos;
using backend.Domain.models;
using backend.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> addUser(CreateUserRequestDto request)
        {
            if(request.username.Length < 3)
            {
                throw new Exception("Username must be at least 3 characters long");
            }

            if(request.password.Length < 8)
            {
                throw new Exception("Password must be at least 8 characters long");
            }

            if(request.name.Length < 0)
            {
                throw new Exception("Name must be filled");
            }

            var existing = await _userRepository.getByUsernameAsync(request.username);

            if (existing != null)
                throw new Exception("Username already registered");

            User user = new User(request.name, request.username, "");
            string hashed = _passwordHasher.HashPassword(user, request.password);


            User newUser = new User(request.name, request.username, hashed);

            return await _userRepository.addUser(newUser);
        }

        public async Task<User> login(LoginRequestDto request)
        {
            var user = await _userRepository.getByUsernameAsync(request.username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.password,
                request.password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            return user;
        }

        public async Task<User> addOrLoginWithGithub(string githubId, string githubUsername)
        {
            User user = await _userRepository.getByGithubId(githubId);

            if (user != null) return user;

            while (true)
            {
                var temp = await _userRepository.getByUsernameAsync(githubUsername);

                if (temp == null) break;

                githubUsername = githubUsername + "_" + new Random().Next(1000);
            }

            User newUser = new User(githubUsername, githubUsername, "")
            {
                githubId = githubId,
            };

            return await _userRepository.addUser(newUser);
        }
    }
}
