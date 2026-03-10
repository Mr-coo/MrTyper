using backend.Application.Dtos;
using backend.Domain.models;
using backend.Domain.Repositories;

namespace backend.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> addUser(CreateUserRequestDto request)
        {
            User user = new User(request.name, request.username, request.password);

            return await _userRepository.addUser(user);
        }
    }
}
