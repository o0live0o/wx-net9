using Microsoft.Extensions.Logging;

namespace wx.Application.Products;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(ILogger<CreateProductCommandValidator> logger)
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Brand).NotEmpty();
        RuleFor(command => command.CategoryId).Must(ValidCategoryId).WithMessage("Caregory Id must be greater then 0.");
    }

    private bool ValidCategoryId(int categoryId)
    {
        return categoryId > 0;
    }
}
