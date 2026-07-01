---
tipo: arquivo
tags: [arquivo, build, powershell, versionamento, nupkg]
arquivo: build.ps1
atualizado: 2026-07-01
---

# build.ps1

Script principal de build, versionamento e empacotamento.

**Caminho:** `build.ps1` (raiz do repositório)

**Requer:** PowerShell 5.1+, .NET 9 SDK (`dotnet`), `nuget` (baixado para `tools\nuget.exe` se ausente), Admin para deploy.

---

## Parâmetro

- **`-Force`** — ignora a detecção de mudanças e sempre incrementa/recompila/empacota.

---

## Passos detalhados

### 1. Lê versão atual do `.nuspec`
`[xml]$spec = Get-Content $nuspec` → `$current` (valida formato `major.minor.build`).

### 2. Calcula newVersion
`$build = [int]$parts[2] + 1` → `$newVersion = "$major.$minor.$build"`.

### 2b. Detecta mudanças
Compara o timestamp mais novo entre fontes (`*.cs`/`*.csproj`/`*.nuspec`/`*.json`/`*.png`, fora de `bin`/`obj`), `*.md` e docs (`LICENSE.txt`, `install/uninstall.ps1`) com o último `.nupkg`. Sem `-Force` e sem mudança → **mantém a versão e sai** (build/pack ignorados).

### 2c. Fecha o GitExtensions
`Stop-Process -Force` nos processos `GitExtensions` (libera a DLL para o deploy).

### 3–4. Bump no `.nuspec` e `.csproj`
`$spec.package.metadata.version = $newVersion` (Save); regex `<Version>…</Version>` → nova versão no csproj.

### 4b. Carimba os READMEs
- `README.md`: atualiza `**Versão atual: …**` e o link do NuGet para a nova versão.
- `README.md` / `README.pt-BR.md` / `README.en-US.md`: atualiza `**Version/Versão:**` e `**Updated/Atualizado em:**` (data de hoje).

<<<<<<< HEAD
### 4c. Carimba o cofre Obsidian
Carimba `versao:`/`atualizado:` do frontmatter e as variantes de `Versão atual` (negrito, tabela e rótulo+crase) nas notas que espelham a versão atual do projeto (Projeto, README espelho, Visão Geral, Versionamento, HOME). Roda **antes** do pack, então o `.nupkg` continua sendo o arquivo mais novo (a detecção de mudanças não entra em loop).
=======
> **Nota:** hoje o script carimba **apenas os READMEs**, não este cofre Obsidian (o irmão CommitMsg carimba ambos). Ao evoluir, incluir o cofre.
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866

### 5. Build
`dotnet build $csproj -c Release --nologo -v minimal`. Conta erros/avisos por regex; falha se `buildExit != 0` ou houver erro.

### 6. Deploy (Admin)
Copia a DLL para `C:\Program Files\GitExtensions\Plugins\` (fallback x86). Pulado com aviso se sem Admin ou pasta ausente.

### 6b. Atualiza `tools\net9.0-windows\`
Copia a DLL para a pasta usada pelo nupkg.

### 7. Pack
`nuget pack $nuspec -OutputDirectory $outDir`, **filtrando o aviso NU5101** (DLL em `lib\` raiz é intencional). Remove `.nupkg` de versões anteriores.

---

## Saídas

| Artefato | Localização |
|---|---|
| DLL compilada | `src\...\bin\Release\net9.0-windows\GitExtensions.Plugins.ZimerfeldLFS.dll` |
| DLL instalada | `C:\Program Files\GitExtensions\Plugins\` |
| DLL no nupkg | `tools\net9.0-windows\` |
| Pacote NuGet | `.\GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` |

---

## Como executar

```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # incrementa versão, build Release, deploy (Admin), nupkg
.\build.ps1 -Force    # empacota mesmo sem mudanças

# Sem Admin (Bash tool / Git Bash):
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"
```

---

## Relacionado

- [[../Sistema/Versionamento]]
- [[../Sistema/Dependências]]
- [[Generate-LfsIcon]]
