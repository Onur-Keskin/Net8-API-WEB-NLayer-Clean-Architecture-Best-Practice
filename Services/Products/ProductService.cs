using App.Repositories;
using App.Repositories.Products;
using App.Services.ExceptionHandlers;
using App.Services.Products.Create;
using App.Services.Products.Update;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository,IUnitOfWork unitOfWork,
        IValidator<CreateProductRequest> createProductRequestValidator,IMapper mapper) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);

            var productsAsDto = products.Select(p=>new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList();//manuel mapleme hızlı çalışır

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();

            #region manuel mapping
            //var productAsDto = products.Select(p => new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList(); 
            #endregion

            var productAsDto =  mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1 - 10 => ilk 10 kayıt skip(0).Take(10)
            // 2 - 10 => 11-20 kayıt skip(10).Take(10)

            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            #region manuel mapping
            //var productAsDto = products.Select(p=>new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList(); 
            #endregion

            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found",HttpStatusCode.NotFound);
            }

            #region manuel mapping
            //var productsAsDto = new ProductDto(product!.Id, product!.Name, product!.Price, product!.Stock); 
            #endregion

            var productAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request) 
        {
            //throw new CriticalException("kritik seviye bir hata meydana geldi");
            #region async manuel service business check
            //async manuel service business check
            var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            } 
            #endregion

            #region async manuel fluent validation business check
            /*
                //async manuel fluent validation business check
                var validationResult = await createProductRequestValidator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    return ServiceResult<CreateProductResponse>.Fail(validationResult.Errors.Select(x=>x.ErrorMessage).ToList());
                }*/
            #endregion

            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id,UpdateProductRequest request)
        {

            // Fast fail : Önce başarısız durumları dön

            //Guard clauses

            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found",HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            productRepository.Delete(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
