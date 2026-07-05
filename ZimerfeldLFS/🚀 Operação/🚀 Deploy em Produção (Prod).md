---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [operacao, runbook, prod, release, nupkg, nuget]
---

# 🚀 Deploy em Produção (Prod)

> 🇺🇸 Read this page in English → [[🚀 Deploy em Produção (Prod) (EN)]]

Como publicar uma nova versão do plugin: gerar o pacote `.nupkg` de release e distribuí-lo (NuGet + GitHub release), de onde o Plugin Manager do GitExtensions o descobre.

## ⚡ TL;DR — o comando único
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1 -Force     # gera o .nupkg de release na raiz do repositório
```
O artefato de release é `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` na raiz. Publique-o no NuGet e anexe-o à GitHub release.

## ⚙️ O que o script faz (em ordem)
`build.ps1` produz o pacote publicável (ver [[🛠️ build.ps1]] e [[🔢 Versionamento]]):
1. Bump da versão (`major.minor.BUILD`) no `.nuspec` e `.csproj`.
2. Carimba versão + data nos READMEs (`README.md` / `.pt-BR` / `.en-US`) e neste cofre.
3. `dotnet build -c Release`.
4. `nuget pack .nuspec` → `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` na raiz, **filtrando o aviso NU5101** (a DLL vai em `lib\` raiz — grupo "any" — para o Plugin Manager conseguir extraí-la).
5. Remove os `.nupkg` de versões anteriores.

## 📦 Publicação (passos manuais após o pack)
1. **NuGet:** `nuget push GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg -Source nuget.org -ApiKey <chave>`.
2. **GitHub release:** criar a release/tag e anexar o `.nupkg` + notas de versão.
3. **Plugin Manager:** com o pacote no NuGet e a dependência marcadora `GitExtensions.Extensibility`, o plugin aparece na busca por *ZimerfeldLFS*.

## 📏 Regras que respeita (GitFlow)
> [!important] Ordem correta do GitFlow (regra do Renato)
> 1. Validar o ambiente de **develop** na branch de **release** (sem erro).
> 2. **Finalizar a release** atualizando `develop` e depois `main`.
> 3. Gerar a **tag** `YYYYMMddhhmm<nome-da-fase>`.
> 4. **Só então** publicar em produção (push NuGet + GitHub release).
> - **Não** fazer deploy a partir da release branch antes de finalizá-la.
> - **Não** commitar/pushar sem pedido explícito.

## ✅ Requisitos
- Tudo do [[💻 Ambiente Local (Dev)]] (.NET 9 SDK, `nuget`).
- Chave de API do NuGet e permissão de release no GitHub (owner `zimerfeld`).

## 🩺 Troubleshooting
- **NU5101** aparece como erro → deve ser apenas aviso e filtrado; confirme que a DLL está em `lib\` raiz no `.nuspec`. Ver [[🧱 Dependências]].
- **Plugin não aparece no Plugin Manager** → verifique a dependência `GitExtensions.Extensibility` no `.nuspec` e se o pacote está indexado no NuGet.
- **Versão não incrementou** → rode com `-Force` (a detecção de mudanças pode ter mantido a versão).

## 🔗 Ligações
- [[💻 Ambiente Local (Dev)]]
- [[🔢 Versionamento]]
- [[🛠️ build.ps1]]
- [[🧱 Dependências]]
