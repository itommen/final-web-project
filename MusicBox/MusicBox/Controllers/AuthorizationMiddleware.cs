// Sharon Grozman - 311429963 
// Elhen Shmailov - 313736639
// Tomer Parizer - 312465602
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicBox.Models;

namespace MusicBox.Controllers
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