---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [operacao, runbook, dev, build, gitextensions, powershell]
---

# 💻 Ambiente Local (Dev)

> 🇺🇸 Read this page in English → [[💻 Ambiente Local (Dev) (EN)]]

Como compilar o plugin, instalá-lo no GitExtensions local e iterar durante o desenvolvimento.

## ⚡ TL;DR — o comando único
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # incrementa build, compila Release, faz deploy no GitExtensions local, gera nupkg
```
Depois, abra o GitExtensions e vá em **Plugins → ZimerfeldLFS**. Para forçar mesmo sem mudanças: `.\build.ps1 -Force`.

## ⚙️ O que o script faz (em ordem)
`build.ps1` (detalhado em [[🛠️ build.ps1]]):
1. Lê a versão atual do `.nuspec` e calcula a próxima (`build +1`).
2. **Detecta mudanças** — sem `-Force` e sem fontes/docs mais novos que o último `.nupkg`, mantém a versão e sai.
3. **Fecha o GitExtensions** em execução (libera a DLL para o deploy).
4. Aplica o bump da versão no `.nuspec` e no `.csproj`; carimba versão + data nos READMEs e neste cofre.
5. `dotnet build -c Release` — falha se houver erro; conta erros/avisos.
6. **Deploy (requer Admin):** copia a DLL para `C:\Program Files\GitExtensions\Plugins\`.
7. Copia a DLL para `tools\net9.0-windows\` e empacota o `.nupkg` (filtrando o aviso **NU5101** intencional).

## 🧩 Instalação manual alternativa (tools/)
```powershell
tools\install.ps1      # requer Admin — copia a DLL para a pasta Plugins (também via Package Manager Console)
tools\uninstall.ps1    # requer Admin — remove a DLL (não afeta o resto do GitExtensions)
```

## 🎛️ Parâmetros / flags
- **`-Force`** — ignora a detecção de mudanças e sempre incrementa/recompila/empacota.

## 🔐 Requisitos
- **.NET 9 SDK** (`dotnet`) no PATH.
- **Administrador** para o passo de deploy (senão é pulado com aviso).
- `nuget` (baixado automaticamente para `tools\nuget.exe` se ausente).
- **Sem Admin** (ex.: via Bash tool): `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"` — compila e empacota; só o deploy é pulado.

## 📏 Regras que respeita
- **GitFlow:** desenvolver na feature branch atual; `build.ps1` só compila/empacota, não faz commit nem push.
- **Não commitar/pushar sem pedido explícito.**

## 🩺 Troubleshooting
- **"Git LFS is NOT installed"** na janela → `git` / `git-lfs` fora do `PATH` (o `LfsService` roda como subprocesso). Ver [[🧱 Dependências]].
- **Deploy pulado** → rode o PowerShell como Administrador ou use `tools\install.ps1`.
- **DLL travada** → o `build.ps1` fecha o GitExtensions antes do deploy; se falhar, feche manualmente.

## 🔗 Ligações
- [[🛠️ build.ps1]]
- [[🔢 Versionamento]]
- [[🧱 Dependências]]
- [[🚀 Deploy em Produção (Prod)]]
