using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widget.LiveAnnouncement.Data;
using Nop.Plugin.Widget.LiveAnnouncement.Domain;
using Nop.Plugin.Widget.LiveAnnouncement.Services;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Widget.LiveAnnouncement.Infrastructure
{
    public partial class DependencyRegister : IDependencyRegistrar
    {
        #region Field
        private const string ContextName = "nop_object_context_live_announcement";
        #endregion

        #region Register

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<AnnouncementService>().As<IAnnouncementService>().InstancePerLifetimeScope();
            builder.RegisterPluginDataContext<LiveAnnouncementObjectContext>(ContextName);
            builder.RegisterType<EfRepository<Announcement>>().As<IRepository<Announcement>>().WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName)).InstancePerLifetimeScope();

        }

        #endregion

        #region DB

        public int Order
        {
            get { return 0; }
        }
        #endregion


    }
}
