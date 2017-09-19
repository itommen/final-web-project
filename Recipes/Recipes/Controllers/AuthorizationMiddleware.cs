using System.Web;
using Recipes.Models;

namespace Recipes.Controllers
{
    public static class AuthorizationMiddleware
    {
        public static bool AdminAuthorized(HttpSessionStateBase Session)
        {
            return (((Client)Session["Client"]) != null && ((Client)Session["Client"]).isAdmin);
        }

        public static bool Authorized(HttpSessionStateBase Session)
        {
            return (((Client)Session["Client"]) != null);
        }
    }
}