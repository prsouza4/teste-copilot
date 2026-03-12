namespace VendeTudo.Identidade.API;

/// <summary>
/// Configuração dos clientes OIDC.
/// </summary>
public class ClienteOIDCConfig
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<string> RedirectUris { get; set; } = new();
    public List<string> PostLogoutRedirectUris { get; set; } = new();
}

/// <summary>
/// Configuração de um usuário inicial.
/// </summary>
public class UsuarioInicialConfig
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Configuração geral do serviço de identidade.
/// </summary>
public class IdentidadeConfig
{
    public List<ClienteOIDCConfig> ClientesOIDC { get; set; } = new();
    public List<UsuarioInicialConfig> UsuariosIniciais { get; set; } = new();
}
