namespace EnderecoApi.Domain.Entities;

public class Endereco
{
    public Guid Id { get; private set; }
    public string Rua { get; private set; } = null!;
    public string Numero { get; private set; } = null!;
    public string Cidade { get; private set; } = null!;
    public string Estado { get; private set; } = null!;
    public string Cep { get; private set; } = null!;

    private Endereco() { }

    public Endereco(string rua, string numero, string cidade, string estado, string cep)
    {
        Id = Guid.NewGuid();
        Rua = rua;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
}
