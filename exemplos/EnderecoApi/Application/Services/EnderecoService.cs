using EnderecoApi.Application.Interfaces;
using EnderecoApi.Domain.Entities;

namespace EnderecoApi.Application.Services;

public class EnderecoService
{
    private readonly IEnderecoRepository _repository;

    public EnderecoService(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Endereco>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Endereco?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

    public async Task AddAsync(Endereco endereco)
    {
        if (await _repository.ExistsAsync(endereco.Rua, endereco.Numero, endereco.Cidade, endereco.Estado, endereco.Cep))
        {
            throw new InvalidOperationException("Endereço já cadastrado.");
        }

        await _repository.AddAsync(endereco);
    }

    public async Task UpdateAsync(Endereco endereco) => await _repository.UpdateAsync(endereco);

    public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
}
