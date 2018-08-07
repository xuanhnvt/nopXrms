using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }
        public int Order
        {
            get { return 1; }
        }
    }
}
