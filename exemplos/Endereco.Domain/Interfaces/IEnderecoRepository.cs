using Endereco.Domain.Entities;

namespace Endereco.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task<bool> ExistsAsync(string rua, string numero, string cidade, string estado, string cep);
    Task AddAsync(Endereco endereco);
    Task<Endereco> GetByIdAsync(Guid id);
    Task<IEnumerable<Endereco>> GetAllAsync();
    Task UpdateAsync(Endereco endereco);
    Task DeleteAsync(Endereco endereco);
}
