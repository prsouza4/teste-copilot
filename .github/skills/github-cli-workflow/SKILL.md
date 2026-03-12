---
name: github-cli-workflow
description: "Use when user mentions git commands, GitHub CLI, creating PRs, raising issues, branch operations, or GitHub automation. Provides complete GitHub CLI and Git workflow knowledge for automated issue tracking, branch management, and PR creation. Auto-loads when git, gh, PR, issue, branch, or merge is mentioned."
---

# GitHub CLI Workflow Skill

## Limites de Tamanho do GitHub

| Campo | Max Chars | Convenção |
|-------|-----------|-----------|
| Título de issue | 255 | Manter abaixo de 60 |
| Título de PR | 255 | Manter abaixo de 60 |
| Corpo do PR | ~64KB | Manter abaixo de 1000 |
| Subject do commit | 72 | Manter abaixo de 50 |

## Detectando Repositório GitHub

Sempre verifique antes de oferecer automação:

```powershell
$remote = git config --get remote.origin.url
if ($remote -match 'github\.com') {
    Write-Host "✅ Repositório GitHub detectado"
}
```

## Fluxo Completo de 5 Passos

### 1. Criar Issue
```powershell
gh issue create --title "feat: Descrição clara" --body "Descrição do que precisa ser feito"
# Capturar número:
$issueUrl = gh issue create --title "..." --body "..."
if ($issueUrl -match '#(\d+)') { $issueNumber = $matches[1] }
```

### 2. Criar Branch (GitFlow)
```bash
# Sempre a partir de develop (nunca main)
git checkout develop && git pull origin develop
git checkout -b feature/$issueNumber-descricao-curta
```

### 3. Criar pasta work/
```powershell
$workDir = "work/ISSUE-$($issueNumber.PadLeft(3,'0'))-descricao"
New-Item -ItemType Directory -Force -Path $workDir
New-Item -ItemType File -Force -Path "$workDir/plan.md"
New-Item -ItemType File -Force -Path "$workDir/result.md"
```

### 4. Commit com referência
```bash
git add .
git commit -m "feat: Descrição

- O que foi feito
- Por que foi feito

Resolves #42

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"
```

### 5. Criar PR (para develop, nunca main)
```powershell
gh pr create `
  --title "feat: Descrição" `
  --body "Fixes #42

## O que mudou
- Item 1
- Item 2

## Testes
✅ dotnet test passou
✅ npm test passou" `
  --base develop
```

## Palavras-chave que fecham issues automaticamente
- `Fixes #42`, `Resolves #42`, `Closes #42`
- ⚠️ DEVE estar no corpo do PR (não só no título)

## Merge do PR
```powershell
$prNumber = gh pr view --json number -q .number
gh pr merge $prNumber --squash --delete-branch
```

## Troubleshooting

| Erro | Solução |
|------|---------|
| `gh: command not found` | Use caminho completo: `& "C:\Program Files\GitHub CLI\gh.exe"` |
| `authentication token missing scopes` | `gh auth refresh -s read:project` |
| `Can not approve your own PR` | Normal — peça a um colega para aprovar |

## Boas Práticas

✅ Sempre verifique se é repo GitHub antes de oferecer automação  
✅ Inclua "Fixes #X" no corpo do PR  
✅ Use squash merge para histórico limpo  
✅ Delete a branch após o merge  
✅ PR aponta para `develop`, não para `main`  
❌ Nunca dê push direto em `main` ou `develop`  
❌ Nunca crie issues sem aprovação do usuário  
