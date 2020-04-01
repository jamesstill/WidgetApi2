using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WidgetApi.Models;

namespace WidgetApi.EFCore
{
    public class WidgetMap : IEntityTypeConfiguration<Widget>
    {
        public void Configure(EntityTypeBuilder<Widget> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Widget");
            builder.HasKey(t => t.Id).IsClustered(false);
            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.Property(t => t.Name);
            builder.Property(t => t.Shape);
        }
    }
}
