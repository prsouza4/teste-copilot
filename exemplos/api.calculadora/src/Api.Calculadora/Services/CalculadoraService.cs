namespace Api.Calculadora.Services;

/// <summary>
/// Interface for calculator operations.
/// </summary>
public interface ICalculadoraService
{
    /// <summary>
    /// Adds two decimal numbers.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Sum of a and b.</returns>
    decimal Somar(decimal a, decimal b);

    /// <summary>
    /// Subtracts second number from first.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Difference of a and b.</returns>
    decimal Subtrair(decimal a, decimal b);

    /// <summary>
    /// Multiplies two decimal numbers.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Product of a and b.</returns>
    decimal Multiplicar(decimal a, decimal b);

    /// <summary>
    /// Divides first number by second.
    /// </summary>
    /// <param name="a">Dividend.</param>
    /// <param name="b">Divisor.</param>
    /// <returns>Quotient of a divided by b.</returns>
    /// <exception cref="DivideByZeroException">Thrown when b is zero.</exception>
    decimal Dividir(decimal a, decimal b);
}

/// <summary>
/// Implementation of calculator operations.
/// </summary>
public class CalculadoraService : ICalculadoraService
{
    /// <inheritdoc />
    public decimal Somar(decimal a, decimal b)
    {
        return a + b;
    }

    /// <inheritdoc />
    public decimal Subtrair(decimal a, decimal b)
    {
        return a - b;
    }

    /// <inheritdoc />
    public decimal Multiplicar(decimal a, decimal b)
    {
        return a * b;
    }

    /// <inheritdoc />
    public decimal Dividir(decimal a, decimal b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Não é possível dividir por zero.");
        }

        return a / b;
    }
}
