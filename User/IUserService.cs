using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;

namespace Shop_Backend.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllUserAsync();
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(int id, CreateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
    }
}