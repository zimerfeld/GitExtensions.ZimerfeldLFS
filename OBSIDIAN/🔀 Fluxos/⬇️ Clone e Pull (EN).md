---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [fluxo, clone, pull, fetch, checkout, etapa3]
---

# ⬇️ Flow: Step 3 — Clone & Pull

Third tab (`tabClone`). Download and restore LFS content after cloning or switching branches.

![[ScreenshotClone.png]]

## 🧭 Context

When collaborators or deploy tools **clone** the repository, Git LFS downloads the heavy files automatically when checking out the branch. This tab is for **fetching or restoring** LFS content manually when needed.

## 🔘 Buttons

```
[git lfs pull]        → RunAsync → _svc.LfsPull()       (downloads the LFS content of the current checkout)
[git lfs fetch --all] → RunAsync → _svc.LfsFetchAll()   (pre-fetches LFS objects of ALL refs)
[git lfs checkout]    → RunAsync → _svc.LfsCheckout()   (populates the working tree from downloaded objects)
[git lfs status]      → RunAsync → _svc.LfsStatus()     (shows the status of LFS objects; refreshAfter:false)
```

| Command | When to use |
|---|---|
| `git lfs pull` | bring the missing LFS content for the current checkout |
| `git lfs fetch --all` | pre-download LFS objects of all refs (e.g.: before going offline) |
| `git lfs checkout` | materialize the working files from objects already downloaded |
| `git lfs status` | inspect the state of LFS objects (query only — does not refresh the UI afterwards) |

Each button shows the exact command and stdout/stderr in the **output console**. The buttons become enabled when there is a valid repository **and** LFS is available.

## 🔗 Related

- [[📤 Track Commit Push (EN)|📤 Track Commit Push]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
