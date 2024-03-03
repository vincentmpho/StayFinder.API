using Microsoft.AspNetCore.Identity;
using StayFinder.API.Models.DTOs.Users;

namespace StayFinder.API.Services.Interface
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VeryifyRefreshToken(AuthResponseDto request);
    }
}
