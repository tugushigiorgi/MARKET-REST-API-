using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptDetailRepository :IReceiptDetailRepository
{
    private readonly TradeMarketDbContext _context;
    public ReceiptDetailRepository(TradeMarketDbContext context)
    {
        _context = context;

    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
    {
        return await  _context.ReceiptsDetails.ToListAsync();
    }

    public  async Task<ReceiptDetail> GetByIdAsync(int id)
    {
        return await _context.ReceiptsDetails.Select(c=>c)
          
            .SingleOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task AddAsync(ReceiptDetail entity)
    {
        await  _context.ReceiptsDetails.AddAsync(entity);
    }

    public void Delete(ReceiptDetail entity)
    {
        _context.ReceiptsDetails.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _context.ReceiptsDetails.Remove(await GetByIdAsync(id));
    }

    public void Update(ReceiptDetail entity)
    {
        _context.ReceiptsDetails.Update(entity);
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
          
        return await _context.ReceiptsDetails
            .OrderBy(c => c.Id)
            
            .Include(c => c.Product)
             
            .ThenInclude(d => d.Category)
            
           
            .Include(d => d.Receipt)
            .ThenInclude(d=>d.Customer)
            .ToListAsync();
        
    }
}