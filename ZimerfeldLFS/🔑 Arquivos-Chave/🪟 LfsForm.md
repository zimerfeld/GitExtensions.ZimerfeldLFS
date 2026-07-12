---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [arquivo, winforms, ui, janela, abas, i18n, threading]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsForm.cs
---

# 🪟 LfsForm.cs

A janela principal — não-modal — que conduz o fluxo de LFS em três etapas.

**Caminho:** `src/GitExtensions.ZimerfeldLFS/LfsForm.cs`

---

## 📜 Declaração e propriedades da janela

```csharp
public sealed class LfsForm : Form
```

| Propriedade | Valor |
|---|---|
| `FormBorderStyle` | `FixedSingle` (não redimensionável) |
| `MaximizeBox` | `false` (com `MinimizeBox = true`) |
| `StartPosition` | `CenterScreen` |
| `Font` | Segoe UI 9 |
| `Icon` | `PluginIcon.ForForm()` |
| `Size` | `720 × (720 + SponsorBanner.PanelHeight)` |

**Construtor** `LfsForm(workingDir, notifyRepoChanged?, openCommitDialog?, openPushDialog?)`: cria o `LfsService`, guarda os delegates, monta a UI (`InitializeComponent`) e popula o `cboRepo` (`LoadRepositories`, só leitura de XML). **Nenhum trabalho git no construtor** → janela instantânea; a primeira sondagem roda no `Shown` via `RefreshStateAsync` em thread de fundo.

---

## 🖼️ Layout (topo → base)

1. **SponsorBanner** (`DockStyle.Top`, topmost) — badges GitHub Sponsors + Ko-fi clicáveis + link "Sobre" à direita (`SponsorBanner.cs`).
2. **Top panel** — rótulo "Diretório de Trabalho", **`cboRepo`** (dropdown de repositório independente) e rótulo de branch.
3. **TabControl** (`DockStyle.Fill`) — 3 abas: `tabInstall`, `tabWorkflow`, `tabClone`.
4. **Log panel** (`DockStyle.Bottom`, altura 150) — cabeçalho ("Saída:" + botão Limpar) e `txtLog` (multiline, read-only, fundo `#1E1E1E`, fonte Consolas).
5. **Bottom panel** — checkbox **Mostrar Debug**, rótulo + dropdown **Idioma**, botão **Fechar** (também `CancelButton`).
6. **StatusStrip** — rótulo de status (`NoGripRenderer` remove o grip de redimensionamento).

---

## 🗂️ As 3 abas

### `tabInstall` (Etapa 1 · Instalação)
- `lblInstallStatus` (status colorido: vermelho = ausente, dourado = disponível, verde = pronto).
- Botões: **Verificar instalação** → `RunAsync(..., () => _svc.GetLfsVersion())`; **`git lfs install`** → `RunAsync("git lfs install", () => _svc.LfsInstall())`.
- `lblInstallHelp` com o texto de ajuda (Homebrew/Chocolatey/binários). Ver [[⚙️ Instalação]].

### `tabWorkflow` (Etapa 2 · Fluxo básico)
- `txtPattern` (com placeholder) + botão **Track** (`DoTrack`; Enter também dispara).
- `lstPatterns` (padrões rastreados) + botão **Untrack** (`DoUntrack`).
- `lstLfsFiles` (arquivos LFS, `ls-files`) com scroll horizontal.
- `commitRow` (base): **Commit…** (`DoCommit`) e **Push…** (`DoPush`). Ver [[📤 Track Commit Push]].

### `tabClone` (Etapa 3 · Clone & Pull)
- `lblCloneHelp` + 4 botões: **`git lfs pull`**, **`git lfs fetch --all`**, **`git lfs checkout`**, **`git lfs status`** (`status` roda com `refreshAfter:false`). Ver [[⬇️ Clone e Pull]].

---

## 📂 Diretório de trabalho independente

`LoadRepositories()` chama `LfsService.GetRepositoriesFromSettings()` (lê o histórico do GitExtensions do XML de settings), insere o working dir inicial no topo se não estiver na lista, e pré-seleciona. `CboRepo_SelectedIndexChanged` troca `_svc.WorkingDir` e dispara `RefreshStateAsync`. `UpdateWorkingDir(newDir)` (chamado pelo plugin quando a janela já existe) adiciona/seleciona o dir. Ver [[📂 Diretório de Trabalho Independente]].

---

## 🪟 Ações que abrem diálogos nativos

- **`DoCommit`** — chama `_openCommitDialog?.Invoke(this, dir)`; loga "concluído / fechado / indisponível" conforme o `bool?`; chama `notifyRepoChanged` e `RefreshStateAsync`.
- **`DoPush`** — chama `_openPushDialog`; se `true`, loga e refaz o estado; senão cai no **fallback** `git push` puro via `RunAsync`.

`DoTrack`/`DoUntrack` rodam `git lfs track/untrack "<glob>"` e, em caso de sucesso, **stageiam o `.gitattributes`** (`_svc.Add(".gitattributes")`).

---

## 🧵 Runner e refresh (threading)

### `RunAsync(title, op, refreshAfter = true)`
Ignora se `_busy` ou se `!EnsureRepoSelected()`. Marca busy, loga `$ <title>`, roda `await Task.Run(op)`, loga `Combined` + ✓/✗ (com `ExitCode`), desmarca busy e (opcional) `RefreshStateAsync`.

### `RefreshStateAsync()`
Sonda numa `Task.Run`: `IsGitRepo`, branch, versão do lfs, disponível, inicializado, padrões, arquivos → `StateSnapshot` → `ApplyState` (atualiza rótulos, listas e habilita/desabilita botões). O status vira "Atualizando…" durante a sonda.

### `Log(message)`
Anexa ao `txtLog` com auto-scroll; faz `BeginInvoke` se `InvokeRequired`.

---

## 🌐 Localização e Debug

- **Idioma:** dropdown Automático/Inglês/Português/Espanhol → `I18n.SetLanguage` → `ApplyLanguage()` recarrega o `Translator` e reescreve todos os textos. `_suppressLangEvent` evita reentrância ao repopular o combo.
- **Mostrar Debug:** `ApplyControlTooltips(show)` percorre a árvore e seta o tooltip = `Name` de cada controle. Persistido em `%APPDATA%\GitExtensions\ZimerfeldLFS.uisettings.json` (`{"showControlIds":…}`).
- **Sobre:** `LinkLabel` no banner → `MessageBox` com o texto localizado (`aboutText`).

### 💾 Arquivos de settings persistidos
| Arquivo (`%APPDATA%\GitExtensions\`) | Conteúdo |
|---|---|
| `ZimerfeldLFS.language.json` | idioma escolhido (`I18n`) |
| `ZimerfeldLFS.uisettings.json` | `showControlIds` (checkbox Debug) |
| `ZimerfeldLFS.debug.log` | log diagnóstico do plugin |

---

## 🔗 Relacionado

- [[🔌 ZimerfeldLfsPlugin]]
- [[⚡ LfsService]]
- [[📂 Diretório de Trabalho Independente]]
- [[🗂️ Fluxo em 3 etapas (abas)]]
- [[🏛️ Arquitetura]]
