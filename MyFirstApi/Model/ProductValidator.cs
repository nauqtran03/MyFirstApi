using FluentValidation;
namespace MyFirstApi.Model

{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
            RuleFor(p=>p.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
