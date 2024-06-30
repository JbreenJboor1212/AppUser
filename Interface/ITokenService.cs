using AppUser.Models;

namespace AppUser.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUserT user);
    }
}
