# API de Endereços

## Descrição

Uma API para gerenciar endereços, incluindo operações de CRUD e validação para evitar endereços duplicados.

## Endpoints

- **POST /addresses**: Cria um novo endereço.
- **PUT /addresses/{id}**: Atualiza um endereço existente.
- **DELETE /addresses/{id}**: Remove um endereço.
- **GET /addresses/{id}**: Obtém um endereço pelo ID.
- **GET /addresses**: Lista todos os endereços.

## Requisitos

- .NET 8 SDK
- Banco de dados em memória (configuração padrão)

## Como executar

1. Clone o repositório.
2. Navegue até `exemplos/api-enderecos/src/API`.
3. Execute `dotnet run`.
4. Acesse os endpoints via Postman ou cURL.
