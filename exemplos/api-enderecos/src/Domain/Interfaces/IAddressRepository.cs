using ApiEnderecos.Domain.Entities;

namespace ApiEnderecos.Domain.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid id);
    Task<Address?> GetByDetailsAsync(string street, string city, string state, string zipCode);
    Task<IEnumerable<Address>> GetAllAsync();
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
}
