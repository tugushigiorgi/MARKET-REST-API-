using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class PersonRepository :IPersonRepository
{
    
    private readonly TradeMarketDbContext _context;
    public PersonRepository(TradeMarketDbContext context)
    {
        _context = context;

    }
    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await  _context.Persons.ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        return await _context.Persons.SingleOrDefaultAsync(customer => customer.Id == id);
    }

    public async  Task AddAsync(Person entity)
    {
        await  _context.Persons.AddAsync(entity);
    }

    public void Delete(Person entity)
    {
        _context.Persons.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _context.Persons.Remove(await GetByIdAsync(id));
    }

    public void Update(Person entity)
    {
        _context.Persons.Update(entity);
    }
}