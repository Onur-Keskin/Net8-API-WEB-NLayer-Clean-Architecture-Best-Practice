﻿using App.Application.Contracts.Persistance;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistance.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            return context.Categories.Include(x => x.Products).AsQueryable();
        }

        Task<List<Category>> ICategoryRepository.GetCategoryWithProductsAsync()
        {
            return context.Categories.Include(x => x.Products).ToListAsync();
        }
    }
}