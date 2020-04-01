using FluentValidation;
using WidgetApi.V1.DTO;

namespace WidgetApi.V1.Validators
{
public class WidgetDTOValidatorCollection : AbstractValidator<WidgetDTO>
{
    public WidgetDTOValidatorCollection()
    {
        RuleFor(i => i.Id).NotNull();
        RuleFor(i => i.Name).NotNull().MaximumLength(50);
        RuleFor(i => i.Shape).NotNull().MaximumLength(50);
    }
}
}
