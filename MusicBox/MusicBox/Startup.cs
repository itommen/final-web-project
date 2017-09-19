using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MusicBox.Startup))]
namespace MusicBox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
