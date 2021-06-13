using Autofac;
using Autofac.Integration.WebApi;
using SampleProject.PublicMessages;
using System.Reflection;
using System.Web.Http;

namespace SampleProject
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      GlobalConfiguration.Configure(Register);
    }

    public static void Register(HttpConfiguration config)
    {
      ContainerBuilder containerBuilder = new ContainerBuilder();
      restlessmedia.Module.Azure.Module module = new restlessmedia.Module.Azure.Module();
      module.RegisterComponents(containerBuilder);
      containerBuilder.RegisterType<PublicMessageTableStore>().As<IPublicMessageStore>();
      containerBuilder.RegisterType<PublicMessageService>().As<IPublicMessageService>();
      containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
      var container = containerBuilder.Build();
      config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}