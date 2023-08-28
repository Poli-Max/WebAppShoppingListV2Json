using System;
using Microsoft.AspNetCore.Mvc;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.Domain.Models;
using Web_App_Shop_V2.Domain.Enum;
using Web_App_Shop_V2.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Web_App_Shop_V2.Domain.ViewModels.Product;
using Newtonsoft.Json;

namespace Web_App_Shop_V2.Controllers;

public class ProductController : Controller
{
	private readonly IProductService _productService;

	public ProductController (IProductService productService)
	{
		this._productService = productService;
	}

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var response = await _productService.GetProductsAll();
        if (response.statusCode == Domain.Enum.StatusCode.OK)
        {
            var productList = response.data ?? new List<Product>();
            var productListJson = JsonConvert.SerializeObject(productList);
            ViewData["ProductListJson"] = productListJson;
            return View(productList);
        }

        return Json(new { error = "Error occurred" });
    }

    [HttpGet]
    public async Task<IActionResult> GetProduct(int id)
    {
        var response = await _productService.GetProductOne(id);
        var productJson = JsonConvert.SerializeObject(response.data);
        ViewData["ProductJson"] = productJson;
        if (response.statusCode == Domain.Enum.StatusCode.OK)
        {
            return View(response.data);
        }

        return Json(new { error = "Product not found" });
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteProduct(int id)
	{
        var response = await _productService.DeleteProduct(id);
        if (response.statusCode == Domain.Enum.StatusCode.OK)
        {
            return RedirectToAction("GetProducts");
        }
        return Json(new { success = false, error = "Product deletion failed" });
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        var newProductModel = new ProductViewModels();
        return View(newProductModel);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateProduct(ProductViewModels model)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.CreateProduct(model);
            if (response.statusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetProduct", new { id = response.data.id });
            }
            else
            {
                return Json(new { success = false, error = response.description });
            }
        }
        return Json(new { success = false, error = "Invalid model data" });
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var response = await _productService.GetProductOne(id);
        if (response.statusCode == Domain.Enum.StatusCode.OK)
        {
            var productViewModel = new ProductViewModels
            {
                id = response.data.id,
                name = response.data.name,
                description = response.data.description,
                price = response.data.price
            };

            return View(productViewModel);
        }

        return Json(new { error = "Product not found" });
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EditProduct(ProductViewModels model)
	{
        if (ModelState.IsValid)
        {
            var response = await _productService.EditProduct(model.id, model);
            if (response.statusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetProduct", new { id = response.data.id });
            }
            else
            {
                return Json(new { success = false, error = response.description });
            }
        }
        return Json(new { success = false, error = "Invalid model data" });
    }

    public async Task<IActionResult> GetProductsSort(string productName, decimal? minPrice, decimal? maxPrice)
    {
        var response = await _productService.GetProductsByFilters(productName, minPrice, maxPrice);
        if (response.statusCode == Domain.Enum.StatusCode.OK)
        {
            ViewData["ProductListJson"] = JsonConvert.SerializeObject(response.data);
            return View("GetProducts", response.data);
        }

        return Json(new { error = "Error occurred" });
    }
}

