---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [arquivo, icone, gdiplus, powershell, design]
arquivo: tools/icon-generator/Generate-LfsIcon.ps1
---

# 🎨 Generate-LfsIcon.ps1

Gerador do ícone do plugin — **100% GDI+ / System.Drawing**, determinístico, sem dependências externas.

**Caminho:** `tools/icon-generator/Generate-LfsIcon.ps1`

---

## 📦 O que produz

Dois PNGs de fundo transparente em `Resources\`:

| Arquivo | Tamanho | Uso |
|---|---|---|
| `icon-128.png` | 128×128 | ícone do pacote NuGet e da janela |
| `ico.png` | 16×16 | ícone do menu Plugins (renderizado em 64 e reduzido para nitidez) |

---

## 🖌️ Composição do desenho

A função `New-LfsIcon -Size` desenha a arte pensada em 128px (com fator de escala `$s`):

1. **Fundo em 4 quadrantes pastel** + cruz divisória — um tom claro por tipo de arquivo grande.
2. **Um "file card" (cartão de arquivo) por quadrante** (`Draw-FileCard`): papel branco com ponta dobrada, contorno colorido, faixa colorida na base e um **glifo** por tipo:
   - **superior-esquerdo — `joystick`** (azul `33,150,243`): joystick de arcade completo (base + capa flexível + haste + bola vermelha com brilho).
   - **superior-direito — `audio`** (roxo `156,39,176`): nota musical — duas colcheias unidas por **feixe duplo** (semicolcheias).
   - **inferior-esquerdo — `movie`** (vermelho `244,67,54`): "tela" arredondada + triângulo de **play** branco.
   - **inferior-direito — `database`** (verde-azulado `0,150,136`): **cilindros** empilhados de banco de dados.
3. **Bomba com pavio aceso ao centro** (estilo Super Mario Bros): halo claro, esfera preta com gradiente radial e brilho especular, bocal no topo, **pavio curvo (bezier)** e **faísca em starburst** (gradiente amarelo→laranja) na ponta.

---

## ▶️ Execução

```powershell
pwsh tools\icon-generator\Generate-LfsIcon.ps1
# -OutDir opcional (default: src\GitExtensions.ZimerfeldLFS\Resources)
```

Salva os dois PNGs e imprime confirmação. Os PNGs são então embutidos no assembly pelo `.csproj` (`ico.png`) e empacotados no nupkg (`icon-128.png` → `icon.png`).

---

## 🎯 Racional de design

O ícone comunica de relance "**arquivos grandes de mídia**" (jogos/áudio/vídeo/dados) e "**explode/pesa**" (a bomba) — os candidatos naturais ao Git LFS. Ver [[💣 Ícone 4 quadrantes + bomba]].

---

## 🔗 Relacionado

- [[💣 Ícone 4 quadrantes + bomba]]
- [[🛠️ build.ps1]]
- [[🪟 LfsForm]] — usa `PluginIcon.ForForm()` (de `ico.png`)
