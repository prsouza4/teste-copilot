# Endereço API

## Descrição

API para gerenciamento de endereços.

## Funcionalidades

- CRUD de endereços
- Validação para evitar endereços duplicados

## Estrutura do Projeto

- **API**: Endpoints e configuração da API.
- **Application**: Casos de uso, comandos, consultas e validações.
- **Domain**: Entidades e interfaces do domínio.
- **Infrastructure**: Persistência e implementação de repositórios.

## Como executar

1. Clone o repositório.
2. Navegue até a pasta `exemplos/Endereco.API`.
3. Execute o comando `dotnet run`.
4. Acesse a documentação da API em `https://localhost:5001/swagger`.

## Tecnologias

- .NET 8
- ASP.NET Core
- Entity Framework Core
- MediatR
- FluentValidation
- In-Memory Database
