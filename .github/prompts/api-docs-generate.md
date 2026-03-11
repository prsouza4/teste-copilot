# API Documentation Generator — Prompt Template
#
# This file is used by the api-docs agent when called from the api-docs.yml workflow.
# It provides task-specific instructions on top of the agent's base definition.

Você é o agente **api-docs**. Um PR foi aberto com alterações em arquivos de backend .NET.

## Sua tarefa

1. Analise o diff do PR para identificar endpoints novos ou modificados
2. Para cada endpoint encontrado, gere um arquivo `.md` de documentação completo em `docs/api/`
3. Atualize o índice `docs/api/README.md`
4. Ao final, poste um comentário no PR listando o que foi criado/atualizado

## Diretrizes

- Leia os arquivos completos dos controllers/endpoints alterados (não apenas o diff)
- Rastreie os DTOs de request e response referenciados
- Rastreie as regras do FluentValidation associadas ao command/DTO
- Inclua exemplos em `curl`, `fetch` JavaScript e tipos TypeScript
- Marque campos como `Required` ou `Optional` com base nas validações encontradas
- Se não encontrar validação explícita para um campo, marque como `—`
- Não documente endpoints internos: `/health`, `/metrics`, `/swagger`, `/favicon`

## Output esperado

Arquivos criados ou atualizados em `docs/api/`, seguindo o padrão:
`docs/api/<recurso>/<método>-<ação>.md`

Exemplo: `docs/api/users/post-create-user.md`

Ao final, liste os arquivos criados/atualizados no formato:

```
[CREATED] docs/api/users/post-create-user.md
[UPDATED] docs/api/README.md
```
