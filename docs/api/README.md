# API Contract Reference

Este diretório contém a documentação consolidada de todos os endpoints das APIs do projeto, gerada automaticamente pelo agente `api-docs` sempre que um endpoint é criado ou modificado.

## Como usar

- **Frontend**: Antes de implementar qualquer UI que consome uma API, verifique se existe o arquivo correspondente aqui. Se existir, use-o como fonte de verdade — não leia o código .NET diretamente.
- **Backend**: Após criar ou modificar um endpoint, o agente `api-docs` é acionado automaticamente via PR para gerar/atualizar a documentação.

## Estrutura

```
docs/api/
├── README.md              ← este arquivo (índice geral)
├── users/
│   ├── post-create-user.md
│   └── get-list-users.md
├── orders/
│   ├── post-create-order.md
│   └── get-order-by-id.md
└── payments/
    └── post-process-payment.md
```

## Formato de cada arquivo

Cada arquivo de endpoint contém:
- Rota, método HTTP e autenticação necessária
- Schema de request e response (JSON)
- Regras de validação
- Exemplos prontos: `curl`, `fetch` e TypeScript

## Índice de Endpoints

> _Nenhum endpoint documentado ainda. Os arquivos serão criados aqui automaticamente quando o agente `api-docs` for acionado._
