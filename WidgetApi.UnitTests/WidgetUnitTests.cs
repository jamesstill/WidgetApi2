using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WidgetApi.EFCore;
using WidgetApi.Models;
using WidgetApi.V2.DTO;
using WidgetApi.V2.Controllers;

namespace WidgetApi.UnitTests
{
    /// <summary>
    /// Testing with EF Core InMemory is the bomb. See:
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
    /// 
    /// Pro tips:
    /// 
    /// Use Guid.NewGuid().ToString() to name databases so each test has a 
    /// unique database name and data doesn't bleed over into another. If you 
    /// hate the idea of creating a new context every time you can always 
    /// call context.Database.EnsureDeleted() in between each test.
    /// </summary>
    [TestClass]
    public class WidgetUnitTests
    {
        Guid id1 = Guid.Parse("076d3b78-7789-40ea-a3cf-0a5b11dab0a9");
        Guid id2 = Guid.Parse("534ad860-bde2-4f35-a56d-b05723d68525");
        Guid id3 = Guid.Parse("0c7f2632-d4cc-4e88-81a1-4af34297e605");

        [TestMethod]
        public void Get_All_Widgets_Found()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                var controller = new WidgetController(context);
                var actionResult = controller.Get().Result;
                var result = actionResult as OkObjectResult;
                var widgets = result.Value as IEnumerable<WidgetDTO>;

                Assert.IsNotNull(result);
                Assert.AreEqual(200, result.StatusCode);
                Assert.IsNotNull(widgets);
                Assert.AreEqual(3, widgets.Count());
            }
        }

        [TestMethod]
        public void Get_One_Widget_Found()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                var controller = new WidgetController(context);
                var actionResult = controller.Get(id2).Result;
                var result = actionResult as OkObjectResult;
                var widget = result.Value as WidgetDTO;

                Assert.IsNotNull(result);
                Assert.AreEqual(200, result.StatusCode);
                Assert.IsNotNull(widget);
                Assert.AreEqual(id2, widget.Id);
            }
        }

        [TestMethod]
        public void Get_All_Widgets_Not_Found()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                var controller = new WidgetController(context);
                var actionResult = controller.Get().Result;
                var result = actionResult as NotFoundObjectResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(404, result.StatusCode);
            }
        }

        [TestMethod]
        public void Post_New_Widget_Success()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                var widgetDTO = new WidgetDTO { Id = Guid.NewGuid(), Name = GetRandomString(), NumberOfGears = 42 };

                var controller = new WidgetController(context);
                var actionResult = controller.Post(widgetDTO).Result;
                var result = actionResult as NoContentResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
            }
        }

        [TestMethod]
        public void Post_New_Widget_Conflict()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                // already exists in the system!
                var widgetDTO = new WidgetDTO { Id = id1, Name = GetRandomString(), NumberOfGears = 42 };

                var controller = new WidgetController(context);
                var actionResult = controller.Post(widgetDTO).Result;
                var result = actionResult as ConflictResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(409, result.StatusCode);
            }
        }

        [TestMethod]
        public void Put_Existing_Widget_Success()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                var widgetDTO = new WidgetDTO { Id = id1, Name = GetRandomString(), NumberOfGears = 42 };

                var controller = new WidgetController(context);
                var actionResult = controller.Put(widgetDTO).Result;
                var result = actionResult as NoContentResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
            }
        }

        [TestMethod]
        public void Put_Existing_Widget_NotFound()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new WidgetContext(options))
            {
                context.Widgets.Add(new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() });
                context.Widgets.Add(new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() });
                context.SaveChanges();

                var widgetDTO = new WidgetDTO { Id = Guid.NewGuid(), Name = GetRandomString(), NumberOfGears = 42 };

                var controller = new WidgetController(context);
                var actionResult = controller.Put(widgetDTO).Result;
                var result = actionResult as NotFoundResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(404, result.StatusCode);
            }
        }

        [TestMethod]
        public void Delete_Existing_Widget_Success()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new WidgetContext(options);
            var w1 = new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() };
            var w2 = new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() };
            var w3 = new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() };
            context.Widgets.Add(w1);
            context.Widgets.Add(w2);
            context.Widgets.Add(w3);
            context.SaveChanges();

            // workaround for EF Core tracking issue 12459. See:
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459
            context.Entry(w1).State = EntityState.Detached;
            context.Entry(w2).State = EntityState.Detached;
            context.Entry(w3).State = EntityState.Detached;
            context.SaveChanges();

            var controller = new WidgetController(context);
            var actionResult = controller.Delete(id2).Result;
            var result = actionResult as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void Delete_Existing_Widget_NotFound()
        {
            var options = new DbContextOptionsBuilder<WidgetContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new WidgetContext(options);
            var w1 = new Widget { Id = id1, Name = GetRandomString(), Shape = GetRandomString() };
            var w2 = new Widget { Id = id2, Name = GetRandomString(), Shape = GetRandomString() };
            var w3 = new Widget { Id = id3, Name = GetRandomString(), Shape = GetRandomString() };
            context.Widgets.Add(w1);
            context.Widgets.Add(w2);
            context.Widgets.Add(w3);
            context.SaveChanges();

            // workaround for EF Core tracking issue 12459. See:
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459
            context.Entry(w1).State = EntityState.Detached;
            context.Entry(w2).State = EntityState.Detached;
            context.Entry(w3).State = EntityState.Detached;
            context.SaveChanges();

            Guid id = Guid.NewGuid(); // not in database!

            var controller = new WidgetController(context);
            var actionResult = controller.Delete(id).Result;
            var result = actionResult as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        private static string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
