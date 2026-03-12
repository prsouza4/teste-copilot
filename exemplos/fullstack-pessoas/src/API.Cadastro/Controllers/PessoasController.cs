using API.Cadastro.Data;
using API.Cadastro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Cadastro.Controllers;

/// <summary>
/// Controller for managing pessoas (people) in the cadastro system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoasController : ControllerBase
{
    private readonly CadastroDbContext _context;

    /// <summary>
    /// Initializes a new instance of PessoasController.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PessoasController(CadastroDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all pessoas.
    /// </summary>
    /// <returns>A list of all pessoas.</returns>
    /// <response code="200">Returns the list of pessoas.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user lacks the required scope.</response>
    [HttpGet]
    [Authorize(Policy = "ReadScope")]
    [ProducesResponseType(typeof(IEnumerable<Pessoa>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
    {
        return await _context.Pessoas.OrderBy(p => p.Nome).ToListAsync();
    }

    /// <summary>
    /// Gets a specific pessoa by ID.
    /// </summary>
    /// <param name="id">The pessoa ID.</param>
    /// <returns>The pessoa with the specified ID.</returns>
    /// <response code="200">Returns the pessoa.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user lacks the required scope.</response>
    /// <response code="404">If the pessoa is not found.</response>
    [HttpGet("{id}")]
    [Authorize(Policy = "ReadScope")]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pessoa>> GetPessoa(Guid id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return NotFound();
        }

        return pessoa;
    }

    /// <summary>
    /// Creates a new pessoa.
    /// </summary>
    /// <param name="dto">The pessoa data.</param>
    /// <returns>The created pessoa.</returns>
    /// <response code="201">Returns the created pessoa.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user lacks the required scope.</response>
    [HttpPost]
    [Authorize(Policy = "WriteScope")]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Pessoa>> CreatePessoa(PessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            DataCadastro = DateTime.UtcNow
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
    }

    /// <summary>
    /// Updates an existing pessoa.
    /// </summary>
    /// <param name="id">The pessoa ID.</param>
    /// <param name="dto">The updated pessoa data.</param>
    /// <returns>The updated pessoa.</returns>
    /// <response code="200">Returns the updated pessoa.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user lacks the required scope.</response>
    /// <response code="404">If the pessoa is not found.</response>
    [HttpPut("{id}")]
    [Authorize(Policy = "WriteScope")]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pessoa>> UpdatePessoa(Guid id, PessoaDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return NotFound();
        }

        pessoa.Nome = dto.Nome;
        pessoa.Email = dto.Email;
        pessoa.Telefone = dto.Telefone;

        await _context.SaveChangesAsync();

        return pessoa;
    }

    /// <summary>
    /// Deletes a pessoa.
    /// </summary>
    /// <param name="id">The pessoa ID.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the pessoa was deleted.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user lacks the required scope.</response>
    /// <response code="404">If the pessoa is not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Policy = "WriteScope")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePessoa(Guid id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return NotFound();
        }

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
