using Api.Calculadora.Services;
using Xunit;

namespace Api.Calculadora.Tests;

/// <summary>
/// Unit tests for CalculadoraService.
/// </summary>
public class CalculadoraServiceTests
{
    private readonly CalculadoraService _sut;

    public CalculadoraServiceTests()
    {
        _sut = new CalculadoraService();
    }

    #region Somar Tests

    [Fact]
    public void Somar_ComNumerosPositivos_RetornaSomaCorreta()
    {
        // Arrange
        decimal a = 10;
        decimal b = 5;

        // Act
        var resultado = _sut.Somar(a, b);

        // Assert
        Assert.Equal(15, resultado);
    }

    [Fact]
    public void Somar_ComNumerosNegativos_RetornaSomaCorreta()
    {
        // Arrange
        decimal a = -10;
        decimal b = -5;

        // Act
        var resultado = _sut.Somar(a, b);

        // Assert
        Assert.Equal(-15, resultado);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10, 0, 10)]
    [InlineData(0, 10, 10)]
    [InlineData(100, 200, 300)]
    [InlineData(-5, 5, 0)]
    [InlineData(1.5, 2.5, 4.0)]
    [InlineData(-1.5, -2.5, -4.0)]
    public void Somar_ComDiversosValores_RetornaSomaCorreta(decimal a, decimal b, decimal esperado)
    {
        // Act
        var resultado = _sut.Somar(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Subtrair Tests

    [Fact]
    public void Subtrair_ComNumerosPositivos_RetornaDiferencaCorreta()
    {
        // Arrange
        decimal a = 10;
        decimal b = 5;

        // Act
        var resultado = _sut.Subtrair(a, b);

        // Assert
        Assert.Equal(5, resultado);
    }

    [Fact]
    public void Subtrair_ComNumerosNegativos_RetornaDiferencaCorreta()
    {
        // Arrange
        decimal a = -10;
        decimal b = -5;

        // Act
        var resultado = _sut.Subtrair(a, b);

        // Assert
        Assert.Equal(-5, resultado);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10, 0, 10)]
    [InlineData(0, 10, -10)]
    [InlineData(200, 100, 100)]
    [InlineData(-5, 5, -10)]
    [InlineData(5, -5, 10)]
    [InlineData(2.5, 1.5, 1.0)]
    public void Subtrair_ComDiversosValores_RetornaDiferencaCorreta(decimal a, decimal b, decimal esperado)
    {
        // Act
        var resultado = _sut.Subtrair(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Multiplicar Tests

    [Fact]
    public void Multiplicar_ComNumerosPositivos_RetornaProdutoCorreto()
    {
        // Arrange
        decimal a = 10;
        decimal b = 5;

        // Act
        var resultado = _sut.Multiplicar(a, b);

        // Assert
        Assert.Equal(50, resultado);
    }

    [Fact]
    public void Multiplicar_ComNumerosNegativos_RetornaProdutoCorreto()
    {
        // Arrange
        decimal a = -10;
        decimal b = -5;

        // Act
        var resultado = _sut.Multiplicar(a, b);

        // Assert
        Assert.Equal(50, resultado);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10, 0, 0)]
    [InlineData(0, 10, 0)]
    [InlineData(10, 10, 100)]
    [InlineData(-5, 5, -25)]
    [InlineData(-5, -5, 25)]
    [InlineData(2.5, 4, 10.0)]
    [InlineData(1.5, 2.0, 3.0)]
    public void Multiplicar_ComDiversosValores_RetornaProdutoCorreto(decimal a, decimal b, decimal esperado)
    {
        // Act
        var resultado = _sut.Multiplicar(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Dividir Tests

    [Fact]
    public void Dividir_ComNumerosPositivos_RetornaQuocienteCorreto()
    {
        // Arrange
        decimal a = 10;
        decimal b = 5;

        // Act
        var resultado = _sut.Dividir(a, b);

        // Assert
        Assert.Equal(2, resultado);
    }

    [Fact]
    public void Dividir_ComNumerosNegativos_RetornaQuocienteCorreto()
    {
        // Arrange
        decimal a = -10;
        decimal b = -5;

        // Act
        var resultado = _sut.Dividir(a, b);

        // Assert
        Assert.Equal(2, resultado);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(100, 10, 10)]
    [InlineData(-10, 5, -2)]
    [InlineData(10, -5, -2)]
    [InlineData(0, 5, 0)]
    [InlineData(7.5, 2.5, 3.0)]
    [InlineData(1, 4, 0.25)]
    public void Dividir_ComDiversosValores_RetornaQuocienteCorreto(decimal a, decimal b, decimal esperado)
    {
        // Act
        var resultado = _sut.Dividir(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void Dividir_PorZero_LancaDivideByZeroException()
    {
        // Arrange
        decimal a = 10;
        decimal b = 0;

        // Act & Assert
        var exception = Assert.Throws<DivideByZeroException>(() => _sut.Dividir(a, b));
        Assert.Contains("zero", exception.Message.ToLower());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(-10)]
    [InlineData(100.5)]
    public void Dividir_QualquerNumeroPorZero_LancaDivideByZeroException(decimal a)
    {
        // Arrange
        decimal b = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _sut.Dividir(a, b));
    }

    #endregion
}
