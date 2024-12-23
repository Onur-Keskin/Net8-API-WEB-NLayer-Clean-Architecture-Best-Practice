using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistance;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Application.ServiceBus;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork,
        IValidator<CreateProductRequest> createProductRequestValidator, IMapper mapper, ICacheService cacheService, 
        IServiceBus serviceBus,ILogger<ProductService> logger) : IProductService
    {
        private const string ProductListCacheKey = "ProductListCacheKey";

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);

            //var productsAsDto = products.Select(p=>new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList();//manuel mapleme hızlı çalışır

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            //cache aside design pattern (önce cache e git sor)
            // 1. cache
            // 2. from db
            // 3. caching data

            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if (productListAsCached is not null)
            {
                logger.LogInformation("Cache'den veri bulundu.");

                return ServiceResult<List<ProductDto>>.Success(productListAsCached);
            }

            var products = await productRepository.GetAllAsync();

            #region manuel mapping
            //var productAsDto = products.Select(p => new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList(); 
            #endregion

            var productAsDto = mapper.Map<List<ProductDto>>(products);


            await cacheService.AddAsync(ProductListCacheKey, productAsDto, TimeSpan.FromMinutes(1));

            logger.LogInformation("Veriler cache'e başarıyla eklendi.");

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1 - 10 => ilk 10 kayıt skip(0).Take(10)
            // 2 - 10 => 11-20 kayıt skip(10).Take(10)

            var products = await productRepository.GetAllPagedAsync(pageNumber,pageSize);

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
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
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
            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.NotFound);
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

            #region manuel mapping
            /*
                var product = new Product()
                {
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock
                };*/
            #endregion

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangeAsync();

            await serviceBus.PublishAsync(new ProductAddedEvent(product.Id,product.Name,product.Price,product.Stock));

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {

            // Fast fail : Önce başarısız durumları dön

            //Guard clauses

            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            var product = mapper.Map<Product>(request);

            product.Id = id;

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                return ServiceResult.Fail("Güncellenecek ürün bulunamadı", HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            productRepository.Delete(product!);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
