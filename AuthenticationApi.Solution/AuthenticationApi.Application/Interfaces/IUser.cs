using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        Task<ApiResponse> Register(UserDTO appUserDTO);
        Task<ApiResponse> Login(LoginDTO loginDTO);
        Task<UserDTO> GetUser(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<ApiResponse> EditUserById(UserDTO appUserDTO);

    }
}
