namespace Api.Calculadora.Models;

/// <summary>
/// Request model for calculator operations.
/// </summary>
public record OperacaoRequest
{
    /// <summary>
    /// First operand.
    /// </summary>
    public decimal A { get; init; }

    /// <summary>
    /// Second operand.
    /// </summary>
    public decimal B { get; init; }
}

/// <summary>
/// Response model for calculator operations.
/// </summary>
public record OperacaoResponse
{
    /// <summary>
    /// Result of the operation.
    /// </summary>
    public decimal Resultado { get; init; }
}

/// <summary>
/// Error response model.
/// </summary>
public record ErroResponse
{
    /// <summary>
    /// Error message.
    /// </summary>
    public string Mensagem { get; init; } = string.Empty;
}
