using System;
using WidgetApi.Models;

namespace WidgetApi.V1.DTO
{
    public static class WidgetDTOExtensions
    {
        /// <summary>
        /// A trivial mapping exercise that could get very involved over time as 
        /// the EF Core entity evolves and introduces breaking changes in the 
        /// REST contract. Putting this mapping exercise in a separate extension 
        /// is a good practice because it keeps your controller methods clean. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static WidgetDTO ToWidgetDTO(this Widget item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new WidgetDTO
            {
                Id = item.Id,
                Name = item.Name,
                Shape = item.Shape
            };
        }

        public static Widget ToWidget(this WidgetDTO item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new Widget
            {
                Id = item.Id,
                Name = item.Name,
                Shape = item.Shape
            };
        }
    }
}
