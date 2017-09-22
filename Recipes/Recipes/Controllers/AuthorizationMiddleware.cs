using System.Web;
using Recipes.Models;

namespace Recipes.Controllers
{
    public static class AuthorizationMiddleware
    {
        public static bool AdminAuthorized(HttpSessionStateBase session)
        {
            return (Client)session["Client"] != null && ((Client)session["Client"]).IsAdmin;
        }

        public static bool Authorized(HttpSessionStateBase session)
        {
            return (Client)session["Client"] != null;
        }
    }
}