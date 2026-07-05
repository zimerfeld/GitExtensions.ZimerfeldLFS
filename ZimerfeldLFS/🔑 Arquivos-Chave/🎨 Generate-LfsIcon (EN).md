---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquivo, icone, gdiplus, powershell, design]
arquivo: tools/icon-generator/Generate-LfsIcon.ps1
---

# 🎨 Generate-LfsIcon.ps1

The plugin's icon generator — **100% GDI+ / System.Drawing**, deterministic, no external dependencies.

**Path:** `tools/icon-generator/Generate-LfsIcon.ps1`

---

## 📦 What it produces

Two transparent-background PNGs in `Resources\`:

| File | Size | Use |
|---|---|---|
| `icon-128.png` | 128×128 | NuGet package and window icon |
| `ico.png` | 16×16 | Plugins menu icon (rendered at 64 and downscaled for sharpness) |

---

## 🖌️ Drawing composition

The `New-LfsIcon -Size` function draws the artwork designed at 128px (with scale factor `$s`):

1. **Background in 4 pastel quadrants** + dividing cross — one light tone per large-file type.
2. **One "file card" per quadrant** (`Draw-FileCard`): white paper with a folded corner, colored outline, colored strip at the base and one **glyph** per type:
   - **top-left — `joystick`** (blue `33,150,243`): complete arcade joystick (base + flexible boot + stick + red ball with highlight).
   - **top-right — `audio`** (purple `156,39,176`): musical note — two eighth notes joined by a **double beam** (sixteenth notes).
   - **bottom-left — `movie`** (red `244,67,54`): rounded "screen" + white **play** triangle.
   - **bottom-right — `database`** (teal `0,150,136`): stacked database **cylinders**.
3. **Bomb with a lit fuse at the center** (Super Mario Bros style): light halo, black sphere with radial gradient and specular highlight, nozzle on top, **curved fuse (bezier)** and a **starburst spark** (yellow→orange gradient) at the tip.

---

## ▶️ Execution

```powershell
pwsh tools\icon-generator\Generate-LfsIcon.ps1
# -OutDir optional (default: src\GitExtensions.ZimerfeldLFS\Resources)
```

Saves the two PNGs and prints a confirmation. The PNGs are then embedded in the assembly by the `.csproj` (`ico.png`) and packaged in the nupkg (`icon-128.png` → `icon.png`).

---

## 🎯 Design rationale

The icon communicates at a glance "**large media files**" (games/audio/video/data) and "**explodes/weighs**" (the bomb) — the natural candidates for Git LFS. See [[💣 Ícone 4 quadrantes + bomba (EN)|💣 Icon: 4 quadrants + bomb]].

---

## 🔗 Related

- [[💣 Ícone 4 quadrantes + bomba (EN)|💣 Icon: 4 quadrants + bomb]]
- [[🛠️ build.ps1 (EN)|🛠️ build.ps1]]
- [[🪟 LfsForm (EN)|🪟 LfsForm]] — uses `PluginIcon.ForForm()` (from `ico.png`)
