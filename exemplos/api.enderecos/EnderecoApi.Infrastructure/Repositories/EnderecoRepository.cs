using EnderecoApi.Domain.Entities;
using EnderecoApi.Domain.Interfaces;
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

    public async Task AddAsync(Endereco endereco)
    {
        await _context.Enderecos.AddAsync(endereco);
        await _context.SaveChangesAsync();
    }
}
