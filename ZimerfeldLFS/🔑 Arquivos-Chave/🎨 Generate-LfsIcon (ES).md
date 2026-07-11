---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquivo, icone, gdiplus, powershell, design]
arquivo: tools/icon-generator/Generate-LfsIcon.ps1
---

# 🎨 Generate-LfsIcon.ps1

Generador del ícono del plugin — **100% GDI+ / System.Drawing**, determinista, sin dependencias externas.

**Ruta:** `tools/icon-generator/Generate-LfsIcon.ps1`

---

## 📦 Qué produce

Dos PNGs de fondo transparente en `Resources\`:

| Archivo | Tamaño | Uso |
|---|---|---|
| `icon-128.png` | 128×128 | ícono del paquete NuGet y de la ventana |
| `ico.png` | 16×16 | ícono del menú Plugins (renderizado en 64 y reducido para nitidez) |

---

## 🖌️ Composición del dibujo

La función `New-LfsIcon -Size` dibuja el arte pensado en 128px (con factor de escala `$s`):

1. **Fondo en 4 cuadrantes pastel** + cruz divisoria — un tono claro por tipo de archivo grande.
2. **Una "file card" (tarjeta de archivo) por cuadrante** (`Draw-FileCard`): papel blanco con esquina doblada, contorno de color, franja de color en la base y un **glifo** por tipo:
   - **superior-izquierdo — `joystick`** (azul `33,150,243`): joystick de arcade completo (base + funda flexible + palanca + bola roja con brillo).
   - **superior-derecho — `audio`** (morado `156,39,176`): nota musical — dos corcheas unidas por una **barra doble** (semicorcheas).
   - **inferior-izquierdo — `movie`** (rojo `244,67,54`): "pantalla" redondeada + triángulo de **play** blanco.
   - **inferior-derecho — `database`** (verde azulado `0,150,136`): **cilindros** apilados de base de datos.
3. **Bomba con mecha encendida en el centro** (estilo Super Mario Bros): halo claro, esfera negra con gradiente radial y brillo especular, boquilla en la parte superior, **mecha curva (bezier)** y **chispa en starburst** (gradiente amarillo→naranja) en la punta.

---

## ▶️ Ejecución

```powershell
pwsh tools\icon-generator\Generate-LfsIcon.ps1
# -OutDir opcional (default: src\GitExtensions.ZimerfeldLFS\Resources)
```

Guarda los dos PNGs e imprime confirmación. Los PNGs luego se embeben en el assembly mediante el `.csproj` (`ico.png`) y se empaquetan en el nupkg (`icon-128.png` → `icon.png`).

---

## 🎯 Racional de diseño

El ícono comunica de un vistazo "**archivos grandes de medios**" (juegos/audio/video/datos) y "**explota/pesa**" (la bomba) — los candidatos naturales para Git LFS. Ver [[💣 Ícone 4 quadrantes + bomba (ES)|💣 Ícono: 4 cuadrantes + bomba]].

---

## 🔗 Relacionado

- [[💣 Ícone 4 quadrantes + bomba (ES)|💣 Ícono: 4 cuadrantes + bomba]]
- [[🛠️ build.ps1 (ES)|🛠️ build.ps1]]
- [[🪟 LfsForm (ES)|🪟 LfsForm]] — usa `PluginIcon.ForForm()` (de `ico.png`)
