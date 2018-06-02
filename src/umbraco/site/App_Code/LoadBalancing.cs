using System.Configuration;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Custom {
	public class RegisterEvents : ApplicationEventHandler {
		protected override void ApplicationStarting (UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
			if (ConfigurationManager.AppSettings["LoadBalancing.IsMaster"] == "true") {
				ServerRegistrarResolver.Current.SetServerRegistrar (new MasterServerRegistrar ());
			} else {
				ServerRegistrarResolver.Current.SetServerRegistrar (new FrontEndReadOnlyServerRegistrar ());
			}
		}
	}

	public class MasterServerRegistrar : IServerRegistrar2 {
		public IEnumerable<IServerAddress> Registrations {
			get { return Enumerable.Empty<IServerAddress> (); }
		}
		public ServerRole GetCurrentServerRole () {
			return ServerRole.Master;
		}
		public string GetCurrentServerUmbracoApplicationUrl () {
			// Must be in this format: http://www.mysite.com/umbraco
			return ConfigurationManager.AppSettings["LoadBalancing.UmbracoApplicationUrl"];
		}
	}

	public class FrontEndReadOnlyServerRegistrar : IServerRegistrar2 {
		public IEnumerable<IServerAddress> Registrations {
			get { return Enumerable.Empty<IServerAddress> (); }
		}
		public ServerRole GetCurrentServerRole () {
			return ServerRole.Slave;
		}
		public string GetCurrentServerUmbracoApplicationUrl () {
			return null;
		}
	}
}