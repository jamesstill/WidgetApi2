using FluentValidation;
using WidgetApi.V2.DTO;

namespace WidgetApi.V2.Validators
{
    public class WidgetDTOValidatorCollection : AbstractValidator<WidgetDTO>
    {
        public WidgetDTOValidatorCollection()
        {
            RuleFor(i => i.Id).NotNull();
            RuleFor(i => i.Name).NotNull().MaximumLength(50);
            RuleFor(i => i.NumberOfGears).NotNull().InclusiveBetween(0, 42);
        }
    }
}
