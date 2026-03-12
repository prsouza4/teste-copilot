---
name: test-driven-development
description: "Enforces Test-Driven Development discipline for all new features, bug fixes, refactoring, and behavior changes. Use before writing ANY implementation code — when a developer says 'implement this', 'write the code', 'add this function', 'fix this bug', or 'build this'. Provides the Red-Green-Refactor cycle, an Iron Law (no production code without a failing test first), and a verification checklist. Auto-loads whenever implementation, coding, building, or feature development is mentioned."
---

# Test-Driven Development (TDD)

## Overview

Write the test first. Watch it fail. Write minimal code to pass.

**Core principle:** If you didn't watch the test fail, you don't know if it tests the right thing.

## The Iron Law

```
NO PRODUCTION CODE WITHOUT A FAILING TEST FIRST
```

Wrote código before the test? Delete it. Start over.

## Red-Green-Refactor

### RED — Escreva o teste que falha
- Um comportamento por teste
- Nome claro descrevendo o comportamento
- Código real (sem mocks a menos que inevitável)

### Verifique RED — Obrigatório
```bash
# .NET
dotnet test --filter "NomeDoTeste"

# Next.js
npm test -- --testNamePattern="nome do teste"
```
Confirme que o teste falha pelo motivo certo (feature faltando, não typo).

### GREEN — Código mínimo
Escreva o **código mais simples possível** para passar o teste. Nada mais.

### Verifique GREEN — Obrigatório
Execute todos os testes. Confirme que todos passam.

### REFACTOR — Limpe
Remova duplicação, melhore nomes. Mantenha os testes verdes.

## Stack deste Projeto

### Backend (.NET 10)
```csharp
// xUnit + FluentAssertions + Moq
[Fact]
public async Task CreateUser_WithValidData_ShouldReturnSuccess()
{
    // Arrange
    var command = new CreateUserCommand("John", "john@email.com");
    
    // Act
    var result = await _handler.Handle(command, CancellationToken.None);
    
    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBeNull();
}
```

### Frontend (Next.js + TypeScript)
```typescript
// Vitest + Testing Library
test('should display error when email is empty', async () => {
  render(<LoginForm />);
  await userEvent.click(screen.getByRole('button', { name: /login/i }));
  expect(screen.getByText('Email é obrigatório')).toBeInTheDocument();
});
```

## Rationalization Rebuttal

| Desculpa | Realidade |
|---------|---------|
| "Muito simples para testar" | Código simples quebra. Teste leva 30 segundos. |
| "Vou testar depois" | Testes escritos depois passam imediatamente. Isso não prova nada. |
| "Já testei manualmente" | Teste manual é ad-hoc. Não pode ser reexecutado. |
| "TDD vai me atrasar" | TDD é mais rápido que depurar. |

## Red Flags — PARE e Recomece

- Código antes do teste
- Teste passa imediatamente (antes de implementar)
- "Só essa vez"
- "Já gastei X horas, deletar é desperdício" (sunk cost fallacy)

**Todos esses significam: Delete o código. Recomece com TDD.**

## Checklist de Verificação

Antes de marcar o trabalho como concluído:
- [ ] Toda nova função/método tem um teste
- [ ] Vi cada teste falhar antes de implementar
- [ ] Cada teste falhou pelo motivo esperado
- [ ] Escrevi código mínimo para passar cada teste
- [ ] Todos os testes passam
- [ ] Nenhum erro ou warning no output
