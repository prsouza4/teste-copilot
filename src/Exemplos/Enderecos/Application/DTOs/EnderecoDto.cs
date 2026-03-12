namespace Exemplos.Enderecos.Application.DTOs;

public record EnderecoDto(
    Guid Id,
    string Rua,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado,
    string Cep
);
