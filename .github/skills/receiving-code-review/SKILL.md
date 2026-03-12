---
name: receiving-code-review
description: "Use when receiving code review feedback or PR review comments, before implementing or accepting any suggestions — especially if feedback seems unclear, technically questionable, or conflicts with prior decisions. Enforces technical rigor over performative agreement. Auto-loads when user mentions code review, PR feedback, reviewer comments, or 'reviewer said to'."
---

# Code Review Reception

## Core Principle

Verifique antes de implementar. Pergunte antes de assumir. Correção técnica acima do conforto social.

## The Response Pattern

```
QUANDO receber feedback de code review:

1. LEIA: Feedback completo sem reagir
2. ENTENDA: Reformule o requisito com suas palavras (ou pergunte)
3. VERIFIQUE: Confira contra a realidade da codebase
4. AVALIE: É tecnicamente correto para ESTA codebase?
5. RESPONDA: Reconhecimento técnico ou contestação fundamentada
6. IMPLEMENTE: Um item por vez, teste cada um
```

## Respostas Proibidas

**NUNCA:**
- "Você está absolutamente certo!" (performativo)
- "Ótimo ponto!" (performativo)
- "Vou implementar isso agora" (antes de verificar)

**EM VEZ:**
- Reformule o requisito técnico
- Faça perguntas esclarecedoras
- Conteste com raciocínio técnico se estiver errado
- Simplesmente comece a trabalhar (ações > palavras)

## Tratando Feedback Externo

Antes de implementar sugestões de revisores externos:
1. Está tecnicamente correto para ESTA codebase?
2. Quebra funcionalidade existente?
3. Existe razão para a implementação atual?
4. Funciona em todas as plataformas/versões?
5. O revisor entende o contexto completo?

## Verificação YAGNI

```
SE revisor sugerir "implementar corretamente" uma feature:
  → Pesquise na codebase se realmente é usada
  SE não usada: "Este endpoint não é chamado. Remover (YAGNI)?"
  SE usada: Então implemente corretamente
```

## Quando Contestar

Conteste quando:
- Sugestão quebra funcionalidade existente
- Revisor não tem contexto completo
- Viola YAGNI (feature não usada)
- Tecnicamente incorreto para esta stack
- Conflita com decisões arquiteturais do projeto

**Como contestar:**
- Use raciocínio técnico, não defensividade
- Faça perguntas específicas
- Referencie testes/código funcionando

## Reconhecendo Feedback Correto

```
✅ "Corrigido. [Breve descrição do que mudou]"
✅ "Bom ponto — [problema específico]. Corrigido em [local]."
✅ [Apenas corrija e mostre no código]

❌ "Você estava certo!"
❌ "Obrigado por pegar isso!"
❌ QUALQUER expressão de gratidão excessiva
```

## Ordem de Implementação

1. Esclareça tudo que está unclear PRIMEIRO
2. Então implemente nesta ordem:
   - Problemas bloqueadores (quebras, segurança)
   - Correções simples (typos, imports)
   - Correções complexas (refactoring, lógica)
3. Teste cada correção individualmente
4. Verifique se não há regressões
