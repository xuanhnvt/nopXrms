using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Nop.Core.Data;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Web.Framework.Infrastructure;
using Nop.Plugin.Xrms.Data;
using Nop.Data;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Services;
using CQRSlite.Routing;
using CQRSlite.Commands;
using CQRSlite.Events;
using Nop.Plugin.Xrms.Cqrs.WriteModel;
using CQRSlite.Caching;
using CQRSlite.Domain;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Handlers;
using CQRSlite.Messages;

namespace Nop.Plugin.Xrms.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_xrms";
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<MaterialGroupService>().As<IMaterialGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<MaterialService>().As<IMaterialService>().InstancePerLifetimeScope();
            builder.RegisterType<SupplierService>().As<ISupplierService>().InstancePerLifetimeScope();
            builder.RegisterType<TableService>().As<ITableService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrentOrderService>().As<ICurrentOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<ClientNotificationService>().As<IClientNotificationService>().InstancePerLifetimeScope();

            //data context
            builder.RegisterPluginDataContext<XrmsObjectContext>("CONTEXT_NAME");

            // all data repositories
            /*builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>))
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME"))
            .InstancePerLifetimeScope();*/

            builder.RegisterType<EfRepository<MaterialGroup>>().As<IRepository<MaterialGroup>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Material>>().As<IRepository<Material>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<ProductRecipe>>().As<IRepository<ProductRecipe>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<MaterialQuantityHistory>>().As<IRepository<MaterialQuantityHistory>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Supplier>>().As<IRepository<Supplier>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<Table>>().As<IRepository<Table>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<CurrentOrder>>().As<IRepository<CurrentOrder>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<CurrentOrderItem>>().As<IRepository<CurrentOrderItem>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<OrderItemNote>>().As<IRepository<OrderItemNote>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<OrderTableMapping>>().As<IRepository<OrderTableMapping>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<CqrsEvent>>().As<IRepository<CqrsEvent>>()
            .WithParameter(ResolvedParameter.ForNamed<IDbContext>("CONTEXT_NAME")).InstancePerLifetimeScope();

            //RegisterCqrs(builder);
        }

        /*private void RegisterCqrs(ContainerBuilder builder)
        {
            builder.Register<Router>(x => new Router()).AsSelf().SingleInstance();
            builder.Register<ICommandSender>(c => c.Resolve<Router>()).SingleInstance();
            builder.Register<IEventPublisher>(c => c.Resolve<Router>()).SingleInstance();
            builder.Register<IHandlerRegistrar>(c => c.Resolve<Router>()).SingleInstance();
            builder.RegisterType<CqrsEventStore>().As<IEventStore>().SingleInstance();
            builder.RegisterType<MemoryCache>().As<ICache>().SingleInstance();
            builder.RegisterType<Session>().As<ISession>().InstancePerLifetimeScope();
            builder.Register<IRepository>(x => new CacheRepository(new Repository(x.Resolve<IEventStore>()),
                                            x.Resolve<IEventStore>(), x.Resolve<ICache>())).InstancePerLifetimeScope();

            var targetAssembly = typeof(CurrentOrderCommandHandlers).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(targetAssembly)
           .Where(type => type.GetInterfaces().Any(y => y.IsAssignableFrom(typeof(IHandler<>)) || y.IsAssignableFrom(typeof(ICancellableHandler<>))))
           .AsImplementedInterfaces()
           .InstancePerDependency();

            //Register routes
            var serviceProvider = ((NopEngine)EngineContext.Current).ServiceProvider;
            var registrar = new RouteRegistrar(serviceProvider);
            registrar.RegisterInAssemblyOf(typeof(CurrentOrderCommandHandlers));
        }*/

        public int Order
        {
            get { return 1; }
        }
    }
}
