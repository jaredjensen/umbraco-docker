using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Sync;

namespace Custom
{
  public class RegisterEvents : ApplicationEventHandler
  {
    protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
      if (System.Environment.GetEnvironmentVariable("Umbraco_IsMaster") == "true")
      {
        ServerRegistrarResolver.Current.SetServerRegistrar(new MasterServerRegistrar());
      }
      else
      {
        ServerRegistrarResolver.Current.SetServerRegistrar(new FrontEndReadOnlyServerRegistrar());
      }
    }
  }

  public class MasterServerRegistrar : IServerRegistrar2
  {
    IEnumerable<IServerAddress> IServerRegistrar.Registrations
    {
      get { return Enumerable.Empty<IServerAddress>(); }
    }

    public ServerRole GetCurrentServerRole()
    {
      return ServerRole.Master;
    }

    public string GetCurrentServerUmbracoApplicationUrl()
    {
      // Must be in this format: http://www.mysite.com/umbraco
      return System.Environment.GetEnvironmentVariable("Umbraco_ApplicationUrl");
    }
  }

  public class FrontEndReadOnlyServerRegistrar : IServerRegistrar2
  {
    public IEnumerable<IServerAddress> Registrations
    {
      get { return Enumerable.Empty<IServerAddress>(); }
    }

    public ServerRole GetCurrentServerRole()
    {
      return ServerRole.Slave;
    }

    public string GetCurrentServerUmbracoApplicationUrl()
    {
      return null;
    }
  }
}