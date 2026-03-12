using EnderecoApi.Application.Interfaces;
using EnderecoApi.Domain.Entities;
using EnderecoApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnderecoApi.Infrastructure.Repositories;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly EnderecoDbContext _context;

    public EnderecoRepository(EnderecoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Endereco>> GetAllAsync() => await _context.Enderecos.ToListAsync();

    public async Task<Endereco?> GetByIdAsync(Guid id) => await _context.Enderecos.FindAsync(id);

    public async Task AddAsync(Endereco endereco)
    {
        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Endereco endereco)
    {
        _context.Enderecos.Update(endereco);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var endereco = await GetByIdAsync(id);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string rua, string numero, string cidade, string estado, string cep)
    {
        return await _context.Enderecos.AnyAsync(e =>
            e.Rua == rua &&
            e.Numero == numero &&
            e.Cidade == cidade &&
            e.Estado == estado &&
            e.Cep == cep);
    }
}
