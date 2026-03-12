using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VendeTudo.Identidade.API;

/// <summary>
/// Contexto do banco de dados para identidade.
/// </summary>
public class IdentidadeDbContext : IdentityDbContext<UsuarioAplicacao>
{
    public IdentidadeDbContext(DbContextOptions<IdentidadeDbContext> options) : base(options)
    {
    }
}

/// <summary>
/// Usuário da aplicação.
/// </summary>
public class UsuarioAplicacao : IdentityUser
{
    public string? NomeCompleto { get; set; }
}
