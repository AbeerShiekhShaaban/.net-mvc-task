using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Task.Startup))]
namespace Task
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
        }
    }
}
