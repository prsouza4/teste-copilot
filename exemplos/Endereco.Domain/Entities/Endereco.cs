namespace Endereco.Domain.Entities;

public class Endereco
{
    public Guid Id { get; private set; }
    public string Rua { get; private set; }
    public string Numero { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string CEP { get; private set; }

    public Endereco(string rua, string numero, string cidade, string estado, string cep)
    {
        Id = Guid.NewGuid();
        Rua = rua;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
    }

    public void Update(string rua, string numero, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
    }
}
