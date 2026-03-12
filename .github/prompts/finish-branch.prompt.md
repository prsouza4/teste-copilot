---
description: 'Use when implementation and verification are complete, before merging or closing out work — when a developer says "I am done", "ready to merge", "wrap up this branch", "create a PR", "finish this feature". Presents 4 structured options: merge locally, create PR, keep branch, or discard. Tests must pass first.'
agent: 'qa'
tools: ['terminal', 'search', 'codebase', 'editFiles', 'changes']
model: 'claude-sonnet-4-5'
---
# Finish Branch — Verificar, Depois Decidir

Este é o portão final antes de merge ou criação de PR.

**Issue doc**: ${input:issue-doc:Caminho para o work doc (ex: work/ISSUE-042-nome/plan.md)}

## Passo 1 — Executar o suite completo de testes

```bash
# .NET
dotnet test

# Next.js
npm test
```

> **Se testes falharem:** Mostre as falhas. "Testes devem passar antes de finalizar esta branch. Corrija as falhas e rode `/finish-branch` novamente."
> **NÃO ofereça opções de merge/PR com testes falhando.**

## Passo 2 — Verificações de qualidade

```bash
# .NET
dotnet build --no-incremental

# Next.js
npx tsc --noEmit && npm run lint
```

Reporte quaisquer erros encontrados.

## Passo 3 — Verificar requisitos do issue

Leia `${input:issue-doc}` e verifique os achados da Fase 5 (Verificação).

Para cada requisito da Fase 1, confirme que está `✅ atendido`.

Se algum requisito não estiver atendido ou verificado:
> "Os seguintes requisitos ainda não foram verificados: [lista]. Corrija antes de finalizar."

## Passo 4 — Apresentar as 4 opções

Quando tudo estiver verde, apresente exatamente estas opções:

```
✅ Implementação completa. O que você gostaria de fazer?

1. Merge para develop localmente — merge da branch feature em develop agora
2. Push e criar Pull Request — push da branch e abrir PR no GitHub (recomendado)
3. Manter a branch como está — cuidarei do merge depois
4. Descartar este trabalho — deletar a branch e todos os commits

Qual opção?
```

## Passo 5 — Executar a opção escolhida

### Opção 1 — Merge local
```bash
git checkout develop
git pull origin develop
git merge feature/ISSUE-XXX-nome
dotnet test && npm test
git branch -d feature/ISSUE-XXX-nome
```

### Opção 2 — Push e PR (Recomendado)
```bash
git push -u origin feature/ISSUE-XXX-nome
gh pr create \
  --title "feat: [Título do Issue]" \
  --body "Fixes #[numero]

## O que mudou
[Sumário das mudanças]

## Testes
✅ dotnet test passou
✅ npm test passou

Ver: work/ISSUE-XXX-nome/result.md" \
  --base develop
```

### Opção 3 — Manter como está
Informe: "Branch `feature/ISSUE-XXX-nome` preservada. Testes passando. Retome a qualquer momento."

### Opção 4 — Descartar
Peça confirmação: "Digite 'descartar' para deletar permanentemente a branch `feature/ISSUE-XXX-nome` e todos os commits."
Só então: `git checkout develop && git branch -D feature/ISSUE-XXX-nome`

## Marcar Issue como Completo

Para Opções 1 e 2, atualize o `work/${input:issue-doc}/result.md`:
```markdown
## Status Final
✅ CONCLUÍDO — [data]
Merge: [PR #numero ou merge local]
```
