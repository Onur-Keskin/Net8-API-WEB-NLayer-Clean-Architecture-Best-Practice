using FluentValidation;
namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            // Name validation
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.");

            //Price validation
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0' dan büyük olmalıdır.");

            //Stock inclusiveBetween validation
            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");
        }
    }
}
