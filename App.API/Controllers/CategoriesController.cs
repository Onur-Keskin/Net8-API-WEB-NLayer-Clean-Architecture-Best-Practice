﻿using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories()=>CreateActionResult(await categoryService.GetAllListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id) => CreateActionResult(await categoryService.GetByIdAsync(id));

        [HttpGet("products")]
        public async Task<IActionResult> GetCategoryWithProducts() => CreateActionResult(await categoryService.GetCategoryWithProductsAsync());

        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetCategoryWithProducts(int id) => CreateActionResult(await categoryService.GetCategoryWithProductsAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request) => CreateActionResult(await categoryService.CreateAsync(request));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest request) => CreateActionResult(await categoryService.UpdateAsync(id, request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id) => CreateActionResult(await categoryService.DeleteAsync(id));

    }
}
