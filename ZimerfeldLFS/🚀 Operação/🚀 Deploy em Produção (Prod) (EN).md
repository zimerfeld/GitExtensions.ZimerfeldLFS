---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [operacao, runbook, prod, release, nupkg, nuget]
---

# 🚀 Production Deploy (Prod)

How to publish a new plugin version: generate the release `.nupkg` package and distribute it (NuGet + GitHub release), from where the GitExtensions Plugin Manager discovers it.

## ⚡ TL;DR — the single command
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1 -Force     # generates the release .nupkg at the repository root
```
The release artifact is `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` at the root. Publish it to NuGet and attach it to the GitHub release.

## ⚙️ What the script does (in order)
`build.ps1` produces the publishable package (see [[🛠️ build.ps1 (EN)|🛠️ build.ps1]] and [[🔢 Versionamento (EN)|🔢 Versioning]]):
1. Version bump (`major.minor.BUILD`) in the `.nuspec` and `.csproj`.
2. Stamps version + date into the READMEs (`README.md` / `.pt-BR` / `.en-US`) and this vault.
3. `dotnet build -c Release`.
4. `nuget pack .nuspec` → `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` at the root, **filtering the NU5101 warning** (the DLL goes in the root `lib\` — the "any" group — so the Plugin Manager can extract it).
5. Removes the `.nupkg` of previous versions.

## 📦 Publishing (manual steps after the pack)
1. **NuGet:** `nuget push GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg -Source nuget.org -ApiKey <key>`.
2. **GitHub release:** create the release/tag and attach the `.nupkg` + release notes.
3. **Plugin Manager:** with the package on NuGet and the marker dependency `GitExtensions.Extensibility`, the plugin appears when searching for *ZimerfeldLFS*.

## 📏 Rules it respects (GitFlow)
> [!important] Correct GitFlow order (Renato's rule)
> 1. Validate the **develop** environment on the **release** branch (no errors).
> 2. **Finish the release** updating `develop` and then `main`.
> 3. Create the **tag** `YYYYMMddhhmm<phase-name>`.
> 4. **Only then** publish to production (NuGet push + GitHub release).
> - **Do not** deploy from the release branch before finishing it.
> - **Do not** commit/push without an explicit request.

## ✅ Requirements
- Everything from [[💻 Ambiente Local (Dev) (EN)|💻 Local Environment (Dev)]] (.NET 9 SDK, `nuget`).
- NuGet API key and GitHub release permission (owner `zimerfeld`).

## 🩺 Troubleshooting
- **NU5101 shows as an error** → it should be a warning and filtered; confirm the DLL is in the root `lib\` in the `.nuspec`. See [[🧱 Dependências (EN)|🧱 Dependencies]].
- **Plugin does not appear in the Plugin Manager** → check the `GitExtensions.Extensibility` dependency in the `.nuspec` and whether the package is indexed on NuGet.
- **Version did not bump** → run with `-Force` (change detection may have kept the version).

## 🔗 Links
- [[💻 Ambiente Local (Dev) (EN)|💻 Local Environment (Dev)]]
- [[🔢 Versionamento (EN)|🔢 Versioning]]
- [[🛠️ build.ps1 (EN)|🛠️ build.ps1]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
