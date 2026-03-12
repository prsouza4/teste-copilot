using ApiEnderecos.Domain.Entities;
using ApiEnderecos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEnderecos.Infrastructure.Persistence;

public class AddressRepository : IAddressRepository
{
    private readonly AddressDbContext _context;

    public AddressRepository(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task<Address?> GetByDetailsAsync(string street, string city, string state, string zipCode)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.Street == street && a.City == city && a.State == state && a.ZipCode == zipCode);
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        return await _context.Addresses.ToListAsync();
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
}
