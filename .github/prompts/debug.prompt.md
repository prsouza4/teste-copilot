---
description: 'Use when a bug, test failure, or unexpected behavior is found — when a developer says "this test is failing", "this is broken", "I found a bug", "something is wrong with X", or when debugging any unexpected output. Requires systematic 4-phase root cause analysis — never guess at a fix.'
agent: 'analyst'
tools: ['terminal', 'search', 'codebase', 'problems', 'editFiles']
model: 'claude-sonnet-4-5'
---
# Systematic Debugging — 4-Phase Root Cause Analysis

**Never guess.** Every change must be backed by a hypothesis proven by a test.

---

## Fase 1 — Reproduzir

Crie uma reprodução mínima e confiável da falha:

```bash
# .NET
dotnet test --filter "NomeDoTeste"

# Next.js
npm test -- --testNamePattern="nome do teste"
```

- Confirme que a falha é determinística (falha toda vez)
- Se intermitente: adicione logging para capturar problemas de timing
- Documente a mensagem de erro exata e stack trace

> **NÃO tente corrigir antes de poder reproduzir de forma confiável.**

---

## Fase 2 — Isolar

Trace o caminho do código para encontrar onde expectativa e realidade divergem:

1. Siga o stack trace para cima — qual função é a raiz?
2. Adicione `console.log` / `_logger.LogDebug` temporários nos checkpoints chave
3. Pergunte: *"Em que ponto exato o valor/estado fica errado?"*
4. Reduza para a menor reprodução possível

```bash
# .NET verbose
dotnet test --verbosity detailed --filter "NomeDoTeste"

# Next.js verbose
npm test -- --verbose --testPathPattern="path/to/test"
```

---

## Fase 3 — Identificar

Forme uma hipótese e verifique com um teste direcionado:

1. Declare a hipótese: *"Acredito que X está errado porque Y"*
2. Escreva um teste mínimo que prove/refute a hipótese
3. Execute o teste — ele falha da forma esperada?
4. Se sim: você encontrou a causa raiz. Prossiga para Fase 4.
5. Se não: revise a hipótese e repita a Fase 3.

---

## Fase 4 — Resolver

Corrija a causa raiz (não o sintoma):

1. Escreva um **teste de regressão** que reproduz o bug ANTES da correção (deve falhar — RED)
2. Confirme que o teste de regressão **falha**
3. Implemente a correção mínima
4. Confirme que o teste de regressão **passa** (GREEN)
5. Execute o suite completo — confirme que não há regressões
6. Commit: `git commit -m "fix: [descrição da causa raiz] (fixes #issue)"`

---

## ⚠️ Regra dos 3 Strikes

Se sua correção falhar 3 vezes seguidas:

```
PARE. Não continue tentando mudanças aleatórias.

1. Documente o estado atual: o que tentou, o que aconteceu, o que esperava
2. Dê um passo atrás — questione seu isolamento da Fase 2
3. Recomece a Fase 2 com olhos frescos
```

Três falhas significam que sua hipótese estava errada.

---

## Anti-Patterns

| Sugestão | Por que está errada | Abordagem correta |
|:---------|:--------------------|:------------------|
| "Só aumenta o timeout" | Esconde o problema real de timing | Descubra por que está lento |
| "Adiciona um retry loop" | Mascara falhas intermitentes | Encontre e corrija a causa da instabilidade |
| "Comenta o assertion" | Remove o valor do teste | Corrija o código para atender o assertion |
| "Talvez se resolva sozinho" | Não vai | Reproduza e trace agora |

---

## Após Corrigir o Bug

- [ ] Teste de regressão existe e está verde
- [ ] Suite completo passa
- [ ] Causa raiz (não sintoma) foi corrigida
- [ ] Mensagem do commit explica a causa raiz
- [ ] Considere: este bug pode existir em outro lugar com código similar?
