using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository :ICustomerRepository
{
    private readonly TradeMarketDbContext _context;
    public CustomerRepository(TradeMarketDbContext context)
    {
        _context = context;

    }
    
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        
        
        return await  _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        return await _context.Customers.SingleOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task AddAsync(Customer entity)
    {
       await  _context.Customers.AddAsync(entity);
    }

    public void Delete(Customer entity)
    {
        _context.Customers.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
         
        _context.Customers.Remove(await GetByIdAsync(id));
    }

    public void Update(Customer entity)
    {
        _context.Customers.Update(entity);
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await _context.Customers
            .Include(c => c.Receipts)
            .ThenInclude(r => r.ReceiptDetails) // Include ReceiptDetails for each Receipt
            .Include(c => c.Person) // Include Person entity
            .ToListAsync();


    }

    public async Task<Customer> GetByIdWithDetailsAsync(int id)
    {


        return await _context.Customers
            .Include(c => c.Receipts)
            .ThenInclude(r => r.ReceiptDetails)
            .Include(c => c.Person)
            .OrderBy(c => c.Id)  
            .SingleOrDefaultAsync(d => d.Id == id);

    }
}