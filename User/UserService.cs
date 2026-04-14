using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;
using Shop_Backend.Models;
using Shop_Backend.Repositories;

namespace Shop_Backend.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }

        public async Task<IEnumerable<UserResponse>> GetAllUserAsync()
        {
            var user = await _repository.GetAllAsync();
            return user.Select(MapToResponse);
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return MapToResponse(user);
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {

                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _repository.AddAsync(user);
            return MapToResponse(user);
        }

        public async Task<bool> UpdateUserAsync(int id, CreateUserRequest request)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            user.Username = request.Username;
            user.Email = request.Email;

            await _repository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            await _repository.DeleteAsync(user);
            return true;
        }
    }
}