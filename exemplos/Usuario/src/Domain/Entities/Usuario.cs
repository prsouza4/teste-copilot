using System;

namespace Usuario.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CPF { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string? Profissao { get; private set; }
    public string Email { get; private set; }

    private Usuario() { }

    public Usuario(string nome, string cpf, DateTime dataNascimento, string email, string? profissao = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;
        Email = email;
        Profissao = profissao;
    }

    public void Atualizar(string nome, string email, string? profissao = null)
    {
        Nome = nome;
        Email = email;
        Profissao = profissao;
    }
}
