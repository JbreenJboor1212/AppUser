using System.Security.Claims;

namespace AppUser.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "ClaimsPrincipal user cannot be null");
            }

            var claim = user.Claims.SingleOrDefault(
                x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));

            if (claim == null)
            {
                throw new InvalidOperationException("Given name claim not found");
            }

            return claim.Value;
        }
    }
}
