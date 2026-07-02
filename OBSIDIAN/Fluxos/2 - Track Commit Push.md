---
tipo: fluxo
tags: [fluxo, track, commit, push, gitattributes, etapa2]
atualizado: 2026-07-01
---

# Fluxo: Etapa 2 — Track / Commit / Push

Segunda aba (`tabWorkflow`). O fluxo básico do LFS: escolher tipos de arquivo, versionar e enviar.

![[ScreenShots/ScreenshotWorkflow.png]]

## Passos

```
1. Digitar padrão glob (ex.: *.psd, *.mp4, *.zip)  →  [Track]
        │  DoTrack → RunAsync("git lfs track \"*.psd\"")
        │     _svc.TrackPattern(p)             → git lfs track "*.psd"
        │     se Ok → _svc.Add(".gitattributes") → stagea o .gitattributes
        ▼
2. RefreshStateAsync repovoa:
        ├─ lstPatterns ← GetTrackedPatterns()   (git lfs track, parseado)
        └─ lstLfsFiles ← GetLfsFiles()          (git lfs ls-files)
        │
        │  (Untrack: selecionar um padrão → [Untrack] → git lfs untrack + stage .gitattributes)
        ▼
3. [Commit…]  → DoCommit → openCommitDialog(this, dir)
        │     abre o StartCommitDialog NATIVO do host no repo do cboRepo
        │     loga concluído / fechado / indisponível; notifyRepoChanged()
        ▼
4. [Push…]    → DoPush → openPushDialog(this, dir)
        │     abre o StartPushDialog NATIVO; se indisponível/cancelado →
        │     fallback RunAsync("git push", _svc.Push())
```

Equivalente em linha de comando:
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

## Detalhes

- **`.gitattributes` staged automaticamente:** rastrear/deixar de rastrear altera o `.gitattributes`; a janela já o coloca em stage (`_svc.Add(".gitattributes")`) para ficar pronto para commit — é a razão de o track ser um passo isolado do commit.
- **Diálogos nativos:** Commit e Push usam os diálogos do próprio GitExtensions (via delegates `openCommit`/`openPush` que resolvem `WithWorkingDirectory(dir)`), então todos os plugins de commit carregam normalmente. Ver [[../Arquivos-Chave/ZimerfeldLfsPlugin]].
- **Enter no campo de padrão** também dispara o Track; padrão vazio → `MessageBox` de aviso.

## Relacionado

- [[1 - Instalação]]
- [[3 - Clone e Pull]]
- [[../Arquivos-Chave/LfsForm]]
- [[../02 - Conhecimento/Git LFS - Conceitos]]
