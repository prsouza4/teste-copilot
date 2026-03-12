using EnderecoApi.Domain.Entities;

namespace EnderecoApi.Application.Interfaces;

public interface IEnderecoRepository
{
    Task<IEnumerable<Endereco>> GetAllAsync();
    Task<Endereco?> GetByIdAsync(Guid id);
    Task AddAsync(Endereco endereco);
    Task UpdateAsync(Endereco endereco);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(string rua, string numero, string cidade, string estado, string cep);
}
