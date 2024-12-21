using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductCategoryRepository :IProductCategoryRepository
{
    
    
    private readonly TradeMarketDbContext _context;
    public ProductCategoryRepository(TradeMarketDbContext context)
    {
        _context = context;

    }
    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return await  _context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory> GetByIdAsync(int id)
    {
        return await _context.ProductCategories.SingleOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task AddAsync(ProductCategory entity)
    {
        await  _context.ProductCategories.AddAsync(entity);
    }

    public void Delete(ProductCategory entity)
    {
        _context.ProductCategories.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _context.ProductCategories.Remove(await GetByIdAsync(id));
    }

    public void Update(ProductCategory entity)
    {
        _context.ProductCategories.Update(entity);
    }
}