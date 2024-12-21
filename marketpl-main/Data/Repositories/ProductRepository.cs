using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository :IProductRepository
{
    private readonly TradeMarketDbContext _context;
    public ProductRepository(TradeMarketDbContext context)
    {
        _context = context;

    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await  _context.Products.ToListAsync();
    }

    public  async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.SingleOrDefaultAsync(customer => customer.Id == id);

    }

    public  async Task AddAsync(Product entity)
    {
        await  _context.Products.AddAsync(entity);
    }

    public void Delete(Product entity)
    {
        _context.Products.Remove(entity);
    }

    public  async Task DeleteByIdAsync(int id)
    {
        _context.Products.Remove(await GetByIdAsync(id));
    }

    public void Update(Product entity)
    {
        _context.Products.Update(entity);
    }

    public  async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
   return await _context.Products.Select(c=>c)
       .Include(d=>d.Category)
       
       .Include(c=>c.ReceiptDetails)
       .ThenInclude(d=>d.Receipt)
       
       .ThenInclude(d=>d.Customer)
       .ThenInclude(d=>d.Receipts)
    .ToListAsync();


    }

    public  async Task<Product> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Products.Select(c=>c)
            
        
            .Include(c=>c.ReceiptDetails)
            .ThenInclude(d=>d.Receipt)
       
            .ThenInclude(d=>d.Customer)
            .ThenInclude(d=>d.Receipts)
            .Include(d=>d.Category)
            .SingleOrDefaultAsync(d=>d.Id==id);

    }
    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.ProductCategoryId == categoryId)
            .ToListAsync();
    }
}