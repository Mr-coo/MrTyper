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
            var existing = await _userRepository.GetByUsernameAsync(request.username);

            if (existing != null)
                throw new Exception("Username already registered");

            User user = new User(request.name, request.username, "");
            string hashed = _passwordHasher.HashPassword(user, request.password);


            User newUser = new User(request.name, request.username, hashed);

            return await _userRepository.addUser(newUser);
        }

        public async Task<User> login(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.username);

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
    }
}
