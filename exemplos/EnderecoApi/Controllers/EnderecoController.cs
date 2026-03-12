using EnderecoApi.Data;
using EnderecoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnderecoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase
{
    private readonly EnderecoDbContext _context;

    public EnderecoController(EnderecoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Endereco>>> GetEnderecos()
    {
        return await _context.Enderecos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Endereco>> GetEndereco(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id);

        if (endereco == null)
        {
            return NotFound();
        }

        return endereco;
    }

    [HttpPost]
    public async Task<ActionResult<Endereco>> CreateEndereco(Endereco endereco)
    {
        var exists = await _context.Enderecos.AnyAsync(e =>
            e.Logradouro == endereco.Logradouro &&
            e.Numero == endereco.Numero &&
            e.Cidade == endereco.Cidade &&
            e.Estado == endereco.Estado);

        if (exists)
        {
            return Conflict("Endereço já cadastrado.");
        }

        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEndereco), new { id = endereco.Id }, endereco);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEndereco(int id, Endereco endereco)
    {
        if (id != endereco.Id)
        {
            return BadRequest();
        }

        _context.Entry(endereco).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnderecoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEndereco(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id);
        if (endereco == null)
        {
            return NotFound();
        }

        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EnderecoExists(int id)
    {
        return _context.Enderecos.Any(e => e.Id == id);
    }
}
