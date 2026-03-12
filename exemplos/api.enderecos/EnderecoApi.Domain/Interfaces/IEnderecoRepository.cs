using EnderecoApi.Domain.Entities;

namespace EnderecoApi.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task AddAsync(Endereco endereco);
}
