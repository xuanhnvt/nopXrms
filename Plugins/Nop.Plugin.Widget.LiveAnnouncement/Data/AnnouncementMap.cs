using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Widget.LiveAnnouncement.Domain;

namespace Nop.Plugin.Widget.LiveAnnouncement.Data
{
    public class AnnouncementMap : NopEntityTypeConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcement");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.IsActive);
            builder.Property(x => x.CreateDate);
        }
    }
}
