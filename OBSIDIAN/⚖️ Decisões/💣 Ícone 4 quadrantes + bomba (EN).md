---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, icone, design, gdiplus]
---

# 💣 ADR — Icon: 4 quadrants + bomb

## 🧭 Context
The plugin needed an icon (for the Plugins menu, the window and the NuGet package) that would communicate at a glance "**large media files**" — the Git LFS domain — and be generated **deterministically and dependency-free** (part of the project's reproducible-build philosophy).

## ✅ Decision
An icon with the area split into **4 equal quadrants**, each a "file card" representing a type of large file, plus a **bomb with a lit fuse** at the center:
- top-left — **arcade joystick** (games), blue;
- top-right — **musical note** with a double beam (audio), purple;
- bottom-left — **play button** (video/movie), red;
- bottom-right — **database cylinders** (datasets), teal;
- center — **bomb with fuse** (Super Mario Bros style): the metaphor for the "weight that blows up" the repository if versioned without LFS.

Drawn 100% in **GDI+ / System.Drawing** by [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]], producing `icon-128.png` (package/window) and `ico.png` (16×16, menu).

## 🔁 Iterations
There were **two design iterations**:
1. initial — "exploding files" (file cards + explosion);
2. final — **4 quadrants** with per-type glyphs + a central **bomb**; in this round the joystick became a **full arcade joystick** and the audio glyph became a **musical note with a double beam**.

## 🔀 Alternatives considered
- **Reuse a generic LFS icon** — no identity of its own.
- **A single symbol** (e.g.: just the bomb) — does not convey "several kinds of large media".
- **4 quadrants + bomb (chosen)** — conveys the domain (games/audio/video/data) **and** the problem LFS solves.

## ⚖️ Consequences
**Positive:**
- Deterministic, no external assets; regenerable at any time.
- Readable at 16×16 (rendered at 64 and downscaled) and distinct on NuGet.

**Negative / trade-offs:**
- Many details compete at 16×16; the high-quality downscale mitigates this, but the icon is richer at 128px.

## 🔗 Related
- [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]]
- [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]]
