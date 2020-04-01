using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WidgetApi.Models;

namespace WidgetApi.EFCore
{
    public static class WidgetData
    {
        public static void Seed(WidgetContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Database.IsSqlite())
            {
                return;
            }

            if (context.Widgets.Any())
            {
                return;
            }

            var widgets = new List<Widget>() {
                new Widget {  Id = Guid.NewGuid(), Name = "Cog", Shape = "Square"},
                new Widget {  Id = Guid.NewGuid(), Name = "Gear", Shape = "Round"},
                new Widget {  Id = Guid.NewGuid(), Name = "Sprocket", Shape = "Octagonal"},
                new Widget {  Id = Guid.NewGuid(), Name = "Pinion", Shape = "Triangular"},
            };

            context.Widgets.AddRange(widgets);
            context.SaveChanges();
        }
    }
}
