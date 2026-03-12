namespace API.Cadastro.Models;

/// <summary>
/// Represents a person in the cadastro system.
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Unique identifier for the person.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Full name of the person.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the person.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the person.
    /// </summary>
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Date when the person was registered.
    /// </summary>
    public DateTime DataCadastro { get; set; }
}

/// <summary>
/// DTO for creating or updating a person.
/// </summary>
public class PessoaDto
{
    /// <summary>
    /// Full name of the person.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the person.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the person.
    /// </summary>
    public string Telefone { get; set; } = string.Empty;
}
