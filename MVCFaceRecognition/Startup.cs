using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCFaceRecognition.Startup))]
namespace MVCFaceRecognition
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
