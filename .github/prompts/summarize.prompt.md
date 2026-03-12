---
description: 'Use when ending a work session or switching context — when a developer says "summarize this session", "save my progress", "capture context", "end of day summary", or before closing the chat. Captures current context, decisions, blockers, and next steps to the Issue doc so the next session can continue seamlessly.'
agent: 'analyst'
tools: ['search', 'codebase', 'changes', 'editFiles']
model: 'claude-sonnet-4-5'
---
# Summarize Session — Preservar Contexto para Próxima Sessão

Captura o contexto da sessão atual para que você (ou um colega) possa continuar exatamente de onde parou.

**Issue doc**: ${input:issue-doc:Caminho para o work doc (ex: work/ISSUE-042-nome/plan.md) ou deixe em branco para auto-detectar}

---

## O que Este Comando Faz

1. **Detecta o issue atual** (pela branch ou doc mais recente)
2. **Analisa a sessão**:
   - O que foi discutido/decidido
   - Qual código foi alterado
   - Quais testes foram escritos/executados
   - Bloqueadores encontrados
   - Perguntas que surgiram
3. **Cria um sumário de sessão** com timestamp, fase, decisões, próximos passos
4. **Atualiza o work doc** na seção "Session Notes"

---

## Passo 1 — Identificar o Issue

Se nenhum caminho fornecido:
```bash
git branch --show-current
# Esperado: feature/ISSUE-042-login-rate-limiting
```

Se não estiver em branch de feature, busque o doc modificado mais recentemente:
```powershell
Get-ChildItem work -Filter "*.md" -Recurse | Sort-Object LastWriteTime -Descending | Select-Object -First 1
```

---

## Passo 2 — Analisar Sessão Atual

```bash
# O que foi alterado
git status --short
git log --oneline -5

# Status dos testes
dotnet test 2>&1 | Select-Object -First 20
# ou
npm test 2>&1 | head -20
```

---

## Passo 3 — Criar Sumário de Sessão

```markdown
### Session Summary — [YYYY-MM-DD HH:MM UTC]

**Desenvolvedor**: [Nome ou $env:USERNAME]
**Fase Atual**: [Fase X: Nome]

#### O que Fizemos
- [Ação 1 — ex: "Implementei middleware de rate limiting"]
- [Ação 2 — ex: "Escrevi 4 testes unitários"]

#### Decisões Tomadas
- [Decisão 1 — ex: "Usar sliding window em vez de fixed window"]
- [Decisão 2 — ex: "Rate limit por email, não por IP"]

#### Arquivos Alterados
- `src/backend/Auth/...` (criado, X linhas)
- `src/frontend/...` (modificado, +X linhas)

#### Status Atual
- ✅ Testes de middleware passando (X/X)
- 🔄 Testes de integração em progresso (X/X passando)
- ⬜ Admin bypass ainda não implementado

#### Bloqueadores / Perguntas Abertas
- ❓ [Pergunta que precisa de resposta]
- ⚠️ [Bloqueador técnico]

#### Próximos Passos
1. [Passo 1]
2. [Passo 2]
3. Executar `/verify` antes do PR
```

---

## Passo 4 — Atualizar o Work Doc

**Encontre ou crie a seção "Session Notes"** no work doc.

Se não existir, adicione ao final do `result.md`:

```markdown
---

## 📝 Session Notes

*(Log cronológico de sessões de trabalho — mais recente primeiro)*

### Session Summary — 2026-03-12 11:00 UTC
...
```

Se a seção existir, **adicione o novo sumário no topo** (mais recente primeiro).

---

## Passo 5 — Mostrar e Confirmar

```
✅ Contexto da sessão salvo em work/ISSUE-042-nome/result.md

Na próxima sessão:
- Leia a seção Session Notes para contexto completo
- Use /status para ver a fase atual
- Continue com a lista de Próximos Passos

Gostaria de:
1. Continuar trabalhando
2. Finalizar (use /finish-branch)
3. Apenas fechar
```

---

## Casos de Uso

- **Fim do dia**: Salve contexto antes de fechar o VS Code
- **Troca de contexto**: Bug de emergência? Salve o estado atual antes
- **Handoff para colega**: Colega pode continuar com contexto completo
- **Após pausa longa**: Retome sabendo exatamente onde parou
