using App.Application.Contracts.Persistance;
using FluentValidation;

namespace App.Application.Features.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            // Name validation
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.");
            //.MustAsync(MustUniqueProductNameAsync).WithMessage("Ürün ismi veritabanında bulunmaktadır.");
            //.Must(MustUniqueProductName).WithMessage("Ürün ismi veritabanında bulunmaktadır.");//bu satır senkron olarak çalışır

            //Price validation
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0' dan büyük olmalıdır.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Ürün kategori değeri 0' dan büyük olmalıdır.");

            //Stock inclusiveBetween validation
            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");
        }

        #region 1.way sync validation
        /*
        //1.way sync validation
        private bool MustUniqueProductName(string name)
        {
            return !_productRepository.Where(x=> x.Name == name).Any();

            //false => bir hata ver
            //true => bir hata yok
        }*/
        #endregion

        #region 2. way async validation
        /*
        //2. way async validation
        private async Task<bool> MustUniqueProductNameAsync(string name, CancellationToken cancellationToken) 
        {
            return ! await _productRepository.Where(x => x.Name == name).AnyAsync(cancellationToken);
        }*/
        #endregion
    }
}
