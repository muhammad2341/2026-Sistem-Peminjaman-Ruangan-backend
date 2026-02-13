using RoomBooking.Api.Models;

namespace RoomBooking.Api.Services
{
    public interface ITokenService
    {
        string GenerateToken(AuthUser user);
        int GetTokenExpiryMinutes();
    }
}
