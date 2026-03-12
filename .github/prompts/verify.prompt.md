---
description: 'Verify an issue is complete, tested, and documented — final check before PR. Use when a developer says "I am done", "ready to verify", "check if complete", or before creating a PR.'
agent: 'qa'
tools: ['terminal', 'search', 'codebase', 'problems', 'changes']
model: 'claude-sonnet-4-5'
---
# Verify — Checklist Antes do PR

Verifica se o issue está completo e pronto para merge.

**Issue doc**: ${input:issue-doc:Caminho para o work doc (ex: work/ISSUE-042-nome/plan.md)}

---

## Verificações

### 1. Testes passando

```bash
# .NET
dotnet test

# Next.js
npm test
```

> **Se testes falharem:** Mostre as falhas. "Testes devem passar antes de finalizar esta branch."
> **NÃO ofereça opções de merge com testes falhando.**

### 2. Qualidade de código

```bash
# .NET
dotnet build --no-incremental

# Next.js
npx tsc --noEmit
npm run lint
```

Reporte quaisquer erros encontrados.

### 3. Requisitos atendidos

Leia `${input:issue-doc}` e verifique cada requisito da Fase 1.

Para cada requisito, confirme se está `✅ atendido`.

Se algum requisito estiver `❌ não atendido`:
> "Os seguintes requisitos ainda não foram verificados: [lista]. Corrija antes de finalizar."

### 4. Documentação atualizada

- [ ] `docs/api/` atualizado (se novos endpoints foram criados)
- [ ] `work/ISSUE-XXX/result.md` preenchido com o que foi feito
- [ ] `projects.json` atualizado (se novos projetos foram criados)

### 5. Sem arquivos temporários

- [ ] Sem `console.log` ou `_logger.LogDebug` deixados para debug
- [ ] Sem arquivos `.bak`, `.tmp` ou código comentado

---

## Relatório de Verificação

Gere um relatório:

```markdown
## Relatório de Verificação — [Data]

### Testes
- .NET: X/Y passando
- Next.js: X/Y passando

### Requisitos
- ✅ [Requisito 1]
- ✅ [Requisito 2]
- ❌ [Requisito 3 — não atendido]

### Veredicto
✅ PRONTO para PR / ❌ BLOQUEADO — [razão]
```

Atualize a Fase 5 no arquivo `${input:issue-doc}` com este relatório.

---

## Se Tudo Estiver Verde

```
✅ Verificação concluída. Use /finish-branch para criar o PR ou merge.
```
