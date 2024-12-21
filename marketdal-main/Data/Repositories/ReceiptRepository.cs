using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptRepository :IReceiptRepository
{
    
    
    private readonly TradeMarketDbContext _context;
    public ReceiptRepository(TradeMarketDbContext context)
    {
        _context = context;

    }
    
    public async Task<IEnumerable<Receipt>> GetAllAsync()
    {
        return await  _context.Receipts.ToListAsync();
    }

    public async Task<Receipt> GetByIdAsync(int id)
    {
        return await _context.Receipts.SingleOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task AddAsync(Receipt entity)
    {
        await  _context.Receipts.AddAsync(entity);
    }

    public void Delete(Receipt entity)
    {
        _context.Receipts.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _context.Receipts.Remove(await GetByIdAsync(id));
    }

    public void Update(Receipt entity)
    {
        _context.Receipts.Update(entity);
    }

    public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
    {
     return await _context.Receipts
            .Include(r => r.ReceiptDetails)
            .ThenInclude(rd => rd.Product)
            .ThenInclude(p => p.Category)
            .Include(r => r.Customer)
            .OrderBy(r => r.Id)
            .ToListAsync();
    }

    public async Task<Receipt> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Receipts
            .Include(r => r.ReceiptDetails)
            .ThenInclude(rd => rd.Product)
            .ThenInclude(p => p.Category)
            .Include(r => r.Customer)
            .OrderBy(r => r.Id)
            .SingleOrDefaultAsync(d => d.Id == id);

    }
}