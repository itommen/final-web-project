using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Recipes.ViewModels
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }

        public ManageLoginsViewModel()
        {
            CurrentLogins = new List<UserLoginInfo>();
            OtherLogins = new List<AuthenticationDescription>();
        }
    }
}