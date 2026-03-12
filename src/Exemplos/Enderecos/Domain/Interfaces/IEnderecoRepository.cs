using Exemplos.Enderecos.Domain.Entities;

namespace Exemplos.Enderecos.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task<bool> ExistsAsync(string rua, string numero, string bairro, string cidade, string estado, string cep);
    Task AddAsync(Endereco endereco);
    Task UpdateAsync(Endereco endereco);
    Task DeleteAsync(Endereco endereco);
    Task<Endereco?> GetByIdAsync(Guid id);
    Task<IEnumerable<Endereco>> GetAllAsync();
}
