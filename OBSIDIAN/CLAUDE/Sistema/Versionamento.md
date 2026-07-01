---
tipo: sistema
tags: [build, versão, nupkg, deploy]
atualizado: 2026-07-01
---

# Versionamento e Build

## Esquema de versão

`major.minor.build` — somente o `build` é incrementado automaticamente pelo `build.ps1`. Major e minor são alterados manualmente.

**Versão atual:** `1.0.1` *(fonte da verdade: `.nuspec` / `.csproj`)*

> [!note] Dicionários de idioma embutidos (sem satellite assemblies)
> Os textos de UI vivem em `Resources/ZimerfeldLFS.en-US.json` e `Resources/ZimerfeldLFS.pt-BR.json`,
> embutidos no **assembly principal** com `WithCulture="false"` + `LogicalName` explícito — assim o
> MSBuild **não** interpreta o infixo `.en-US`/`.pt-BR` como cultura (o que os desviaria para
> satellite assemblies), preservando o deploy de **DLL única**. Lidos em runtime por `I18n`.

## Ciclo build.ps1

```
build.ps1  [-Force]
  │
  ├─ 1. Lê versão atual do .nuspec
  ├─ 2. Calcula newVersion (build +1)
  ├─ 2b. Detecta mudanças (fontes/docs mais novos que o último .nupkg); sem -Force e sem
  │      mudança → mantém versão e sai (build/pack ignorados)
  ├─ 2c. Fecha o GitExtensions se estiver em execução
  ├─ 3. Bump no .nuspec  ← <version>
  ├─ 4. Bump no .csproj  ← <Version>
  ├─ 4b. Carimba versão + data no topo dos READMEs (README.md / .pt-BR / .en-US)
  ├─ 4c. Carimba versão + data neste cofre Obsidian (notas que espelham a versão)
  ├─ 5. dotnet build -c Release
  ├─ 6. Copia DLL → C:\Program Files\GitExtensions\Plugins\  (requer Admin)
  ├─ 6b. Copia DLL → tools\net9.0-windows\  (para o nupkg)
  ├─ 7. nuget pack .nuspec → .nupkg na raiz (filtra o aviso NU5101)
  └─ 7b. Remove .nupkg de versões anteriores
```

> [!warning] Aviso **NU5101** é intencional
> A DLL é empacotada em `lib\` **raiz** (grupo "any" que o Plugin Manager extrai), não em
> `lib\net9.0-windows\`. Isso gera o aviso NU5101, que o `build.ps1` **filtra de propósito** no
> `nuget pack`. Detalhe em [[Dependências]] e no `.nuspec`.

<!-- -->

> Requer o **.NET 9 SDK** (`dotnet`) e, para o deploy, permissão de **Administrador**. Sem Admin, o passo de deploy é pulado com aviso; `nuget` é baixado para `tools\nuget.exe` se não estiver no PATH.

## Arquivos versionados

| Arquivo | Campo atualizado |
|---|---|
| `GitExtensions.ZimerfeldLFS.nuspec` | `<version>` |
| `GitExtensions.ZimerfeldLFS.csproj` | `<Version>` (e `**Versão atual**` + link do NuGet no README.md) |
| `README.md` / `README.pt-BR.md` / `README.en-US.md` | `**Version/Versão:**` e `**Updated/Atualizado em:**` |

> **Nota:** o `build.ps1` carimba versão/data nos **READMEs** (seção 4b) **e** neste cofre Obsidian (seção 4c) — as notas que espelham a versão atual (Projeto, README espelho, Visão Geral, Versionamento, HOME) ficam em sincronia automática, igual ao irmão CommitMsg.

## Instalação / desinstalação manual

```powershell
tools\install.ps1        # requer Admin — copia a DLL para a pasta Plugins (também via PMC)
tools\uninstall.ps1      # requer Admin — remove a DLL (não afeta o resto do GitExtensions)
```

## Relacionado

- [[../Arquivos-Chave/build.ps1]]
- [[Dependências]]
