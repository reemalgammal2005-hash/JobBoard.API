
using JobBoard.Common.DTOs.Auth;

namespace JobBoard.BLL.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
