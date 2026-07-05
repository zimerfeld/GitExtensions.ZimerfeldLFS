---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, git, git-lfs, ponteiros, gitattributes]
---

# 🗃️ Git LFS — Concepts

## 📌 Summary
**Git Large File Storage (LFS)** is an open-source Git extension that replaces **large files** (audio, video, datasets, PSDs, binaries) with **lightweight text pointers** inside the repository. The real file content is hosted on a **separate remote server**, which **speeds up cloning** and **avoids bloating** the repository history.

## 🔀 Pointer vs. real content
On commit, Git stores only a small pointer file (a few lines: version, SHA-256 oid, size) instead of the heavy file. The binary blob goes to the remote's **LFS store**. On checkout, Git LFS swaps the pointer for the real content (smudge); on commit, it swaps the content for the pointer (clean) — the **`filter.lfs.clean`/`smudge` filters** are installed by `git lfs install`.

## 📄 `.gitattributes` and tracking
Tracked patterns live in the **`.gitattributes`** file (versioned). To track a type:
```bash
git lfs track "*.psd"     # writes "*.psd filter=lfs diff=lfs merge=lfs -text" to .gitattributes
git add .gitattributes    # the .gitattributes MUST be committed
```
> [!important] Remember to commit the `.gitattributes`
> Without a versioned `.gitattributes`, collaborators do not know those files are LFS. That is why the window **stages the `.gitattributes` automatically** after track/untrack — see [[📤 Track Commit Push (EN)|📤 Track Commit Push]].

## 🧰 Commands per step

| Step | Command | Effect |
|---|---|---|
| Installation | `git lfs install` | writes the LFS filters to the global config (once per machine) |
| | `git lfs version` | checks whether `git-lfs` is available |
| Tracking | `git lfs track "<glob>"` | adds the pattern to `.gitattributes` |
| | `git lfs untrack "<glob>"` | removes the pattern |
| | `git lfs track` | lists the tracked patterns |
| | `git lfs ls-files` | lists the files managed by LFS |
| Clone/Pull | `git lfs pull` | downloads the LFS content of the current checkout |
| | `git lfs fetch --all` | pre-fetches LFS objects for all refs |
| | `git lfs checkout` | populates the working tree from the downloaded objects |
| | `git lfs status` | shows the status of the LFS objects |

## 💾 Storage and clone
`git clone` of a repo with LFS downloads the heavy files **on demand** when checking out the branch (via smudge). `fetch --all` pre-downloads everything (useful before going offline). Since the history keeps only pointers, cloning does not drag down every old version of the binaries — hence the gain in speed and size.

## 🔌 Where this shows up in the plugin
Each method of [[⚡ LfsService (EN)|⚡ LfsService]] maps directly to one of these commands; the three tabs of [[🪟 LfsForm (EN)|🪟 LfsForm]] group them by step. See [[🗂️ Fluxo em 3 etapas (abas) (EN)|🗂️ 3-step flow (tabs)]].

## 🔗 Related
- [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[📤 Track Commit Push (EN)|📤 Track Commit Push]]
