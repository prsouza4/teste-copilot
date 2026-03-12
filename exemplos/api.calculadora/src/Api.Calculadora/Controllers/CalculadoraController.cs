using Api.Calculadora.Models;
using Api.Calculadora.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Calculadora.Controllers;

/// <summary>
/// Controller for calculator operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CalculadoraController : ControllerBase
{
    private readonly ICalculadoraService _calculadoraService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalculadoraController"/> class.
    /// </summary>
    /// <param name="calculadoraService">Calculator service instance.</param>
    public CalculadoraController(ICalculadoraService calculadoraService)
    {
        _calculadoraService = calculadoraService;
    }

    /// <summary>
    /// Adds two numbers.
    /// </summary>
    /// <param name="request">Operation request with two operands.</param>
    /// <returns>Result of addition.</returns>
    [HttpPost("somar")]
    [ProducesResponseType(typeof(OperacaoResponse), StatusCodes.Status200OK)]
    public IActionResult Somar([FromBody] OperacaoRequest request)
    {
        var resultado = _calculadoraService.Somar(request.A, request.B);
        return Ok(new OperacaoResponse { Resultado = resultado });
    }

    /// <summary>
    /// Subtracts second number from first.
    /// </summary>
    /// <param name="request">Operation request with two operands.</param>
    /// <returns>Result of subtraction.</returns>
    [HttpPost("subtrair")]
    [ProducesResponseType(typeof(OperacaoResponse), StatusCodes.Status200OK)]
    public IActionResult Subtrair([FromBody] OperacaoRequest request)
    {
        var resultado = _calculadoraService.Subtrair(request.A, request.B);
        return Ok(new OperacaoResponse { Resultado = resultado });
    }

    /// <summary>
    /// Multiplies two numbers.
    /// </summary>
    /// <param name="request">Operation request with two operands.</param>
    /// <returns>Result of multiplication.</returns>
    [HttpPost("multiplicar")]
    [ProducesResponseType(typeof(OperacaoResponse), StatusCodes.Status200OK)]
    public IActionResult Multiplicar([FromBody] OperacaoRequest request)
    {
        var resultado = _calculadoraService.Multiplicar(request.A, request.B);
        return Ok(new OperacaoResponse { Resultado = resultado });
    }

    /// <summary>
    /// Divides first number by second.
    /// </summary>
    /// <param name="request">Operation request with two operands.</param>
    /// <returns>Result of division or error if dividing by zero.</returns>
    [HttpPost("dividir")]
    [ProducesResponseType(typeof(OperacaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Dividir([FromBody] OperacaoRequest request)
    {
        try
        {
            var resultado = _calculadoraService.Dividir(request.A, request.B);
            return Ok(new OperacaoResponse { Resultado = resultado });
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(new ErroResponse { Mensagem = ex.Message });
        }
    }
}
