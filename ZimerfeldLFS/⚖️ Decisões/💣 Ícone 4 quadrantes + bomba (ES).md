---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, icone, design, gdiplus]
---

# 💣 ADR — Icono: 4 cuadrantes + bomba

## 🧭 Contexto
El plugin necesitaba un icono (para el menú Plugins, la ventana y el paquete NuGet) que comunicara de un vistazo "**archivos grandes de medios**" — el dominio de Git LFS — y que se generara de forma **determinista y sin dependencias** (parte de la filosofía de build reproducible del proyecto).

## ✅ Decisión
Un icono con el área dividida en **4 cuadrantes iguales**, cada uno una "tarjeta de archivo" (file card) que representa un tipo de archivo grande, más una **bomba con mecha encendida** en el centro:
- superior-izquierdo — **joystick de arcade** (juegos), azul;
- superior-derecho — **nota musical** con doble barra (audio), morado;
- inferior-izquierdo — **botón de play** (video/película), rojo;
- inferior-derecho — **cilindros de base de datos** (datasets), verde azulado;
- centro — **bomba con mecha** (estilo Super Mario Bros): la metáfora del "peso que hace explotar" el repositorio si se versiona sin LFS.

Dibujado 100% en **GDI+ / System.Drawing** por [[🎨 Generate-LfsIcon (ES)|🎨 Generate-LfsIcon]], produciendo `icon-128.png` (paquete/ventana) e `ico.png` (16×16, menú).

## 🔁 Iteraciones
Hubo **dos iteraciones** de diseño:
1. inicial — "archivos explotando" (tarjetas de archivo + explosión);
2. final — **4 cuadrantes** con glifos por tipo + **bomba** central; en esta ronda el joystick se convirtió en un **joystick de arcade completo** y el glifo de audio se convirtió en una **nota musical con doble barra**.

## 🔀 Alternativas consideradas
- **Reutilizar un icono genérico de LFS** — sin identidad propia.
- **Un único símbolo** (por ejemplo, solo la bomba) — no comunica "varios tipos de medios grandes".
- **4 cuadrantes + bomba (elegida)** — comunica el dominio (juegos/audio/video/datos) **y** el problema que resuelve LFS.

## ⚖️ Consecuencias
**Positivas:**
- Determinista, sin assets externos; regenerable en cualquier momento.
- Legible en 16×16 (renderizado en 64 y reducido) y distinto en NuGet.

**Negativas / trade-offs:**
- Muchos detalles compiten en 16×16; la reducción de alta calidad lo mitiga, pero el icono es más rico en 128px.

## 🔗 Relacionado
- [[🎨 Generate-LfsIcon (ES)|🎨 Generate-LfsIcon]]
- [[📦 GitExtensions.ZimerfeldLFS (ES)|📦 GitExtensions.ZimerfeldLFS]]
