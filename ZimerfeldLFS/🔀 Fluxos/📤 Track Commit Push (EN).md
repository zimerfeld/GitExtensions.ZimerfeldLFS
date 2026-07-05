---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [fluxo, track, commit, push, gitattributes, etapa2]
---

# 📤 Flow: Step 2 — Track / Commit / Push

Second tab (`tabWorkflow`). The basic LFS workflow: choose file types, commit and push.

![[ScreenshotWorkflow.png]]

## 👣 Steps

```
1. Type a glob pattern (e.g.: *.psd, *.mp4, *.zip)  →  [Track]
        │  DoTrack → RunAsync("git lfs track \"*.psd\"")
        │     _svc.TrackPattern(p)             → git lfs track "*.psd"
        │     if Ok → _svc.Add(".gitattributes") → stages .gitattributes
        ▼
2. RefreshStateAsync repopulates:
        ├─ lstPatterns ← GetTrackedPatterns()   (git lfs track, parsed)
        └─ lstLfsFiles ← GetLfsFiles()          (git lfs ls-files)
        │
        │  (Untrack: select a pattern → [Untrack] → git lfs untrack + stage .gitattributes)
        ▼
3. [Commit…]  → DoCommit → openCommitDialog(this, dir)
        │     opens the host's NATIVE StartCommitDialog on the cboRepo repository
        │     logs completed / closed / unavailable; notifyRepoChanged()
        ▼
4. [Push…]    → DoPush → openPushDialog(this, dir)
        │     opens the NATIVE StartPushDialog; if unavailable/cancelled →
        │     fallback RunAsync("git push", _svc.Push())
```

Command-line equivalent:
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

## 🔍 Details

- **`.gitattributes` staged automatically:** tracking/untracking changes `.gitattributes`; the window stages it right away (`_svc.Add(".gitattributes")`) so it is ready for commit — that is why track is a step separate from commit.
- **Native dialogs:** Commit and Push use GitExtensions' own dialogs (via the `openCommit`/`openPush` delegates that resolve `WithWorkingDirectory(dir)`), so all commit plugins load normally. See [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]].
- **Enter in the pattern field** also triggers Track; empty pattern → warning `MessageBox`.

## 🔗 Related

- [[⚙️ Instalação (EN)|⚙️ Installation]]
- [[⬇️ Clone e Pull (EN)|⬇️ Clone and Pull]]
- [[🪟 LfsForm (EN)|🪟 LfsForm]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
