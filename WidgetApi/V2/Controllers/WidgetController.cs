using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WidgetApi.EFCore;
using WidgetApi.Models;
using WidgetApi.V2.DTO;
using WidgetApi.Validation;

namespace WidgetApi.V2.Controllers
{
    [Validate]
    [ApiController]
    [Route("api/v2/[controller]")]
    public class WidgetController : ControllerBase
    {
        private readonly WidgetContext _context;

        public WidgetController(WidgetContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));

            _context.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WidgetDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            // get the database entities using EF Core
            var items = await _context.Widgets
                .OrderBy(w => w.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (items == null || items.Count == 0)
            {
                var message = "There are no widgets in the system!";
                return NotFound(message);
            }

            // shape the database entities to the return type
            var list = items.Select(i => i.ToWidgetDTO());
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WidgetDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _context.Widgets
                .FirstOrDefaultAsync(w => w.Id == id)
                .ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item.ToWidgetDTO());
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put([FromBody] WidgetDTO widgetDTO)
        {
            if (widgetDTO == null)
            {
                throw new ArgumentNullException(nameof(widgetDTO));
            }

            var item = await _context.Widgets
                .FirstOrDefaultAsync(w => w.Id == widgetDTO.Id)
                .ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = widgetDTO.Name;
            item.Shape = "42";

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Post([FromBody] WidgetDTO widgetDTO)
        {
            if (widgetDTO == null)
            {
                throw new ArgumentNullException(nameof(widgetDTO));
            }

            var item = await _context.Widgets
                .FirstOrDefaultAsync(w => w.Id == widgetDTO.Id)
                .ConfigureAwait(false);

            if (item != null)
            {
                return Conflict(); // 409
            }

            _context.Widgets.Add(widgetDTO.ToWidget());

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = _context.Widgets.FirstOrDefault(w => w.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Widgets.Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }
    }
}
