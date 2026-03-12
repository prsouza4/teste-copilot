namespace Exemplos.Enderecos.Domain.Entities;

public class Endereco
{
    public Guid Id { get; private set; }
    public string Rua { get; private set; }
    public string Numero { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }

    private Endereco() { }

    public Endereco(string rua, string numero, string bairro, string cidade, string estado, string cep)
    {
        Id = Guid.NewGuid();
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }

    public void Update(string rua, string numero, string bairro, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
}
