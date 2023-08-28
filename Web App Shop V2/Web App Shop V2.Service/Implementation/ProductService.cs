using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.Domain.Enum;
using Web_App_Shop_V2.Domain.Models;
using Web_App_Shop_V2.Domain.Response;
using Web_App_Shop_V2.Domain.ViewModels.Product;
using Web_App_Shop_V2.Service.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_App_Shop_V2.Service.Implementation;

public class ProductService : IProductService
{
    private readonly IBaseRepository<Product> _productRepository;

    public ProductService(IBaseRepository<Product> productRepository)
    {
        this._productRepository = productRepository;
    }

    public async Task<IBaseResponse<Product>> GetProductOne(int id) // метод получения подробной информации о продукте
    {
        var baseResponce = new BaseResponse<Product>();
        try
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
            if (product == null)
            {
                baseResponce.description = "User no found";
                baseResponce.statusCode = StatusCode.UserNotFound;
                return baseResponce;
            }

            var productNew = new Product
            {
                id = product.id,
                name = product.name,
                description = product.description,
                price = product.price,
            };

            return new BaseResponse<Product>()
            {
                statusCode = StatusCode.OK,
                data = productNew
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Product>()
            {
                description = $"[GetProduct] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<Product>>> GetProductsAll() // метод получения всех продуктов
    {
        var baseResponse = new BaseResponse<IEnumerable<Product>>();
        try
        {
            var productsQuery = _productRepository.GetAll();
            var products = await productsQuery.ToListAsync();

            if (!products.Any())
            {
                baseResponse.description = "Нет элементов";
                baseResponse.statusCode = StatusCode.OK;
            }
            else
            {
                baseResponse.data = products;
                baseResponse.statusCode = StatusCode.OK;
            }

            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<Product>>()
            {
                description = $"[GetProducts] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<ProductViewModels>> CreateProduct(ProductViewModels model) // метод создания продуктов
    {
        var baseResponse = new BaseResponse<ProductViewModels>();
        try
        {
            var existingProducts = await _productRepository.GetAll().ToListAsync();
            int nextProductId = existingProducts.Any() ? existingProducts.Max(x => x.id) + 1 : 0;

            var product = new Product()
            {
                id = nextProductId,
                name = model.name,
                description = model.description,
                price = model.price
            };

            await _productRepository.Create(product);

            return new BaseResponse<ProductViewModels>()
            {
                statusCode = StatusCode.OK,
                data = new ProductViewModels
                {
                    id = product.id,
                    name = product.name,
                    description = product.description,
                    price = product.price
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ProductViewModels>()
            {
                description = $"[CreateProduct] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<bool>> DeleteProduct(int id) // метод удаления продукта
    {
        var baseResponce = new BaseResponse<bool>()
        {
            data = true
        };
        try
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
            if (product == null)
            {
                baseResponce.description = "Product no found";
                baseResponce.statusCode = StatusCode.ProductNotFound;
                baseResponce.data = false;

                return baseResponce;
            }

            await _productRepository.Delete(product);

            baseResponce.statusCode = StatusCode.OK;
            baseResponce.description = "Error";

            return baseResponce;
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                description = $"[DeleteProduct] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<Product>> EditProduct(int id, ProductViewModels model) // метод изменения продукта
    {
        var baseResponce = new BaseResponse<Product>();
        try
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
            if (product == null)
            {
                baseResponce.description = "Product no found";
                baseResponce.statusCode = StatusCode.ProductNotFound;

                return baseResponce;
            }

            product.name = model.name;
            product.description = model.description;
            product.price = model.price;

            await _productRepository.Update(product);


            baseResponce.statusCode = StatusCode.OK;
            baseResponce.data = product;
            baseResponce.description = "Error";

            return baseResponce;
        }
        catch (Exception ex)
        {
            return new BaseResponse<Product>()
            {
                description = $"[EditProduct] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<List<Product>>> GetProductsByFilters(string productName, decimal? minPrice, decimal? maxPrice) // метод сортировки продуктов
    {
        var baseResponse = new BaseResponse<List<Product>>();
        try
        {
            var productsQuery = _productRepository.GetAll();  // Получаем IQueryable<Product> из репозитория

            if (!string.IsNullOrEmpty(productName))
            {
                productsQuery = productsQuery.Where(x => x.name.StartsWith(productName));
            }

            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(x => x.price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(x => x.price <= maxPrice.Value);
            }

            var filteredProducts = await productsQuery.ToListAsync();  // Извлекаем данные из базы

            if (!filteredProducts.Any())
            {
                baseResponse.description = "Нет элементов, удовлетворяющих фильтрам";
                baseResponse.statusCode = StatusCode.OK;
            }
            else
            {
                baseResponse.data = filteredProducts;
                baseResponse.statusCode = StatusCode.OK;
            }

            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<Product>>()
            {
                description = $"[GetProductsByFilters] : {ex.Message}",
                statusCode = StatusCode.InternalServerError
            };
        }
    }
}

