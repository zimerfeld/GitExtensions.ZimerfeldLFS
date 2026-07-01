---
tipo: referencia
<<<<<<< HEAD
criado: 2026-07-01
atualizado: 2026-07-01
=======
criado: 2026-06-01
atualizado: 2026-06-02
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866
tags: [fatos-chave, referencia]
---

# 🔑 Fatos-Chave

> [!tip] Leia primeiro
> Verdades estáveis e sempre úteis. Atualize quando algo mudar.

## 👤 Usuário
- Nome: **Renato Zimerfeld**
- Email: renato.zimerfeld@gmail.com
- Idioma de trabalho: **Português (BR)**
- Ver [[Renato]] para preferências detalhadas

## 📁 Caminhos importantes
<<<<<<< HEAD
- Cofre de memória (este): `C:\GitExtensions\GitExtensions.ZimerfeldLFS\OBSIDIAN\CLAUDE`
- Raiz do projeto: `C:\GitExtensions\GitExtensions.ZimerfeldLFS`
- `C:\GitExtensions\GitExtensions.ZimerfeldLFS` **é** o repositório do projeto [[GitExtensions.ZimerfeldLFS]] (C#, plugin GitExtensions para Git LFS)
- GitExtensions instalado em: `C:\Program Files\GitExtensions\`
- Plugins do GitExtensions: `C:\Program Files\GitExtensions\Plugins\`
- Settings do GitExtensions (fonte do histórico de repos): `%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings`
- Settings do plugin: `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json` / `ZimerfeldLFS.uisettings.json` / `ZimerfeldLFS.debug.log`
- Projetos irmãos: [[GitExtensions.ZimerfeldTree]] (árvore de branches) e [[GitExtensions.ZimerfeldCommitMsg]] (mensagens de commit), cada um com seu próprio cofre
=======
- Cofre de memória (este): `C:\GitExtensions\ZimerfeldCommitMsg\OBSIDIAN\CLAUDE`
- Raiz de projetos: `C:\GitExtensions\ZimerfeldCommitMsg`
- `C:\GitExtensions\ZimerfeldCommitMsg` **é** o repositório do projeto [[GitExtensions.ZimerfeldCommitMsg]] (C#, plugin GitExtensions)
- GitExtensions instalado em: `C:\Program Files\GitExtensions\`
- Plugins do GitExtensions: `C:\Program Files\GitExtensions\Plugins\`
- Projeto irmão: `C:\NUGET\ZimerfeldTree` — plugin [[GitExtensions.ZimerfeldTree]] (árvore de branches), tem seu próprio cofre de memória
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866

## 🛠️ Ferramentas ativas
- **RTK (Rust Token Killer)** — proxy CLI que economiza tokens. Ver [[RTK]]
- **Obsidian** — este cofre de memória
- **Claude Code** no Windows (shell: PowerShell / git bash)

## 📐 Convenções
- Datas: `AAAA-MM-DD`
<<<<<<< HEAD
- **Este projeto** é um **plugin MEF de GitExtensions** que gerencia **Git LFS** numa janela dedicada em 3 etapas (Instalação / Fluxo básico / Clone & Pull), com **diretório de trabalho independente** do host. Ver [[GitExtensions.ZimerfeldLFS]] e [[../02 - Conhecimento/Git LFS - Conceitos]]
- Requisitos de runtime: **`git`** e **`git-lfs`** no `PATH`
- Versionamento `major.minor.BUILD`, build incrementado automaticamente pelo `build.ps1`
- Licença: **CC BY-NC-ND 4.0 © 2026 Zimerfeld**
=======
- Convenção de commits: descrição em **pt-BR** iniciada por **verbo** (ex.: `Implementa…`, `Corrige…`). É o que este projeto gera — classifica as mudanças pelos tipos **Conventional Commits** (`feat`/`fix`/`docs`/…) só para **escolher o verbo**, **sem** imprimir o prefixo `tipo:` (também em **inglês**, conforme SO/override — ver [[../Decisoes/Suporte Multilíngue PT-EN]] e [[../02 - Conhecimento/Geração de mensagem - Conventional Commits]])
- Versionamento `major.minor.BUILD`, build incrementado automaticamente pelo `build.ps1`
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866
- Commits co-authored quando solicitado push
- Não fazer commit/push sem pedido explícito
