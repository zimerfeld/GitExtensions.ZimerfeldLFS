# GitExtensions.ZimerfeldLFS — Guia Técnico de Operação (Português)

![Ícone](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

**Versão:** 1.0.4 — **Atualizado em:** 2026-07-04

> Este documento é o manual detalhado, passo a passo, de **como operar** a janela do plugin.
> Para uma visão de alto nível veja o [README principal](README.md) · Versão em inglês: [README.en-US.md](README.en-US.md).

---

## ⚡ Resumo executivo

- **O que é:** plugin (MEF) para o GitExtensions que expõe o **Git LFS** numa **janela dedicada e não-modal**, guiando por um **fluxo de 3 etapas** — *Instalação* → *Track/Commit/Push* → *Clone/Pull*.
- **Problema que resolve:** o Git LFS é poderoso, mas dependente de linha de comando e propenso a erros de configuração (`git lfs install`, `track`, `.gitattributes`). O plugin transforma esse fluxo em cliques, com **log visível** de cada comando `git`/`git lfs` executado.
- **Diferenciais:** janela persistente (não interrompe o trabalho no host); **diretório de trabalho independente** do repositório ativo do GitExtensions; **i18n** (Automático / EN-US / PT-BR); ícone próprio; banner de patrocínio (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, alvo **net9.0-windows**, empacotado como **nupkg**; build e versionamento automatizados via `build.ps1`.
- **Estado atual:** versão **`1.0.2`** — funcional, com **36 testes unitários (xUnit)** cobrindo o `LfsService`.
- **Público-alvo:** desenvolvedores e times que versionam ativos grandes (jogos, mídia, datasets de ML) e já usam o GitExtensions no Windows.

---

## Anatomia da janela

Ao abrir **Plugins → ZimerfeldLFS**, aparece uma janela não-modal de tamanho fixo (720 px de largura) com as seguintes regiões, de cima para baixo:

| Região | O que faz |
| --- | --- |
| **Banner de patrocínio** | Links para GitHub Sponsors / Ko-fi e o diálogo *Sobre*. |
| Dropdown **Working Directory** | Seleciona o repositório contra o qual todo comando roda — escolhido de forma **independente** do GitExtensions host. |
| Rótulo **Branch** | Mostra a branch atual do repositório selecionado (ou *não é um repositório git*). |
| **Abas** | O fluxo em três etapas: *Instalação*, *Fluxo básico*, *Clone & Pull*. |
| Console **Output** | Um log escuro, somente leitura e monoespaçado que ecoa cada comando e sua saída bruta. |
| **Barra inferior** | Alternador *Mostrar Debug*, seletor de **idioma** e **Fechar**. |
| **Barra de status** | Feedback *Pronto* / *Executando <comando>…* / *Atualizando…*. |

A janela **não** executa nenhuma operação git ao abrir, então aparece instantaneamente. A primeira sondagem (versão do LFS, padrões rastreados, arquivos, branch) roda em uma thread de segundo plano logo após a janela ser exibida.

## Escolhendo o diretório de trabalho

O dropdown **Working Directory** é preenchido com o histórico de repositórios do GitExtensions (lido de `GitExtensions.settings`), então qualquer repo que você já abriu no host fica disponível aqui — independentemente de qual repo o host mostra no momento.

- Selecionar outra entrada re-sonda imediatamente aquele repositório e atualiza todas as abas.
- Se o host trocar de repositório com a janela aberta, o dropdown acompanha.
- Se o caminho selecionado não for uma árvore de trabalho git, o rótulo **Branch** exibe *não é um repositório git* e os botões do fluxo ficam desabilitados.

## Lendo o console Output

Todo botão passa por um único executor que:

1. Ecoa o comando com o prefixo `$ ` (ex.: `$ git lfs pull`).
2. Imprime a saída combinada (stdout + stderr) do processo.
3. Termina com `✓ Done.` em caso de sucesso (código de saída 0) ou `✗ Failed (exit code N).` em caso de falha.

Enquanto um comando roda, o painel de diretório e as abas ficam desabilitados, o cursor vira ampulheta e a barra de status exibe *Executando <comando>…*. Cliques concorrentes são ignorados até o comando atual terminar. Use **Limpar** para esvaziar o log.

---

## Etapa 1 · Instalação

![ZimerfeldLFS — aba Instalação](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotInstallation.png)

Esta aba confirma que o Git LFS está presente e inicializado para a sua conta de usuário.

**Linha de status (topo, colorida):**

| Cor | Significado |
| --- | --- |
| 🟢 Verde escuro | LFS **pronto** — detectado (ex.: `git-lfs/3.7.1`) *e* inicializado para o seu usuário. |
| 🟡 Dourado escuro | LFS **disponível** mas ainda não inicializado — clique em `git lfs install`. |
| 🔴 Vermelho escuro | LFS **ausente** — não encontrado no `PATH`. |

"Inicializado para o seu usuário" é detectado verificando se `filter.lfs.clean` existe na sua configuração git global (o que o `git lfs install` grava).

**Botões:**

- **Verificar instalação** → roda `git lfs version` e atualiza a linha de status.
- **`git lfs install`** → roda `git lfs install`, configurando os filtros do LFS para a sua conta de usuário. Executa **uma vez por máquina**; fica desabilitado quando o LFS está ausente.

**Instalando o Git LFS manualmente** (apenas se o status estiver vermelho):

- **Windows / macOS** — normalmente já vem junto com o Git.
- **macOS / Linux** — `brew install git-lfs`
- **Windows** — `choco install git-lfs`
- **Qualquer SO** — binários oficiais em [git-lfs.com](https://git-lfs.com).

---

## Etapa 2 · Fluxo básico — track / commit / push

![ZimerfeldLFS — aba Fluxo Básico](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotBasicWorkflow.png)

Esta aba é o ciclo do dia a dia: dizer ao LFS quais arquivos gerenciar e, depois, fazer commit e push.

### Rastrear um padrão

Digite um **padrão glob** na caixa de texto e clique em **Rastrear** (ou pressione <kbd>Enter</kbd>). O plugin executa:

```bash
git lfs track "<padrão>"
git add .gitattributes   # adicionado ao stage automaticamente em caso de sucesso
```

Padrões comuns: `*.psd`, `*.mp4`, `*.zip`, `*.bin`, `assets/**`. Um padrão vazio é recusado com um aviso.

### Lista de padrões rastreados + Untrack

A lista **Padrões rastreados** espelha o `git lfs track` (com a anotação final `(.gitattributes)` removida). Selecione uma entrada e clique em **Untrack** (Deixar de rastrear) para executar:

```bash
git lfs untrack "<padrão>"
git add .gitattributes   # adicionado ao stage automaticamente em caso de sucesso
```

Untrack sem nenhuma seleção é recusado com um aviso.

### Arquivos gerenciados pelo LFS

A lista **Arquivos gerenciados pelo LFS** espelha o `git lfs ls-files` — cada linha é `<oid> <*|-> <caminho>` (onde `*` significa que o arquivo está materializado localmente e `-` significa que é apenas um ponteiro). O rótulo mostra a contagem e é atualizado após cada comando.

### Commit & Push

- **Commit…** abre o **diálogo nativo de commit do GitExtensions** in-process (para carregar todos os plugins de commit) para o repo selecionado. O console registra se houve commits, se o diálogo foi fechado sem commitar, ou se estava indisponível.
- **Push…** abre o **diálogo nativo de push do GitExtensions**. Se nenhum diálogo de push do host estiver disponível (ou for cancelado), o plugin recorre a um `git push` simples na branch atual e registra o fallback.

O equivalente manual completo deste ciclo:

```bash
git lfs track "*.psd"
git add .gitattributes
git add meu_arquivo_grande.psd
git commit -m "Adiciona arquivo PSD grande"
git push origin main
```

Todos os controles de *track / untrack / commit / push* ficam desabilitados a menos que o diretório de trabalho seja um repo git **e** o LFS esteja disponível.

---

## Etapa 3 · Clone & Pull

![ZimerfeldLFS — aba Clone & Pull](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotCloningPulling.png)

Quando colaboradores ou ferramentas de deploy clonam o repositório, o Git LFS baixa os arquivos pesados automaticamente ao fazer checkout da branch. Para buscar ou restaurar o conteúdo LFS depois, cada botão corresponde a exatamente um comando, sempre executado no repo do dropdown **Working Directory**:

| Botão | Comando | Finalidade |
| --- | --- | --- |
| **git lfs pull** | `git lfs pull` | Baixa o conteúdo LFS do checkout atual. |
| **git lfs fetch --all** | `git lfs fetch --all` | Pré-busca os objetos LFS de **todas** as refs (não mexe na árvore de trabalho). |
| **git lfs checkout** | `git lfs checkout` | Popula os arquivos da árvore de trabalho a partir dos objetos já baixados. |
| **git lfs status** | `git lfs status` | Mostra o status dos objetos LFS (**não** dispara atualização de estado). |

Uma sequência típica de restauração após um clone que pulou os objetos LFS: **git lfs fetch --all** → **git lfs checkout**, ou simplesmente **git lfs pull**.

---

## Barra inferior

- **Mostrar Debug** — revela o `Name` interno de cada controle como tooltip (auxílio de diagnóstico); a escolha é persistida em `ZimerfeldLFS.uisettings.json`.
- **Idioma** — *Automático* (segue o SO), *Inglês* ou *Português*. Trocar re-rotula toda a janela imediatamente.
- **Fechar** — fecha a janela (também é a ação <kbd>Esc</kbd> / Cancelar). A janela é não-modal, então o GitExtensions continua utilizável enquanto ela está aberta.

## Solução de problemas

| Sintoma | Causa provável & solução |
| --- | --- |
| Linha de status vermelha / *ausente* | Git LFS não está no `PATH`. Instale (veja a Etapa 1), reabra a aba, clique em **Verificar instalação**. |
| Botões do fluxo acinzentados | O caminho selecionado não é um repo git, ou o LFS está indisponível. Escolha um repo válido no dropdown. |
| **Branch** exibe *não é um repositório git* | O dropdown aponta para uma pasta que não é uma árvore de trabalho git. |
| Commit informa *indisponível* | O diálogo nativo de commit do GitExtensions não pôde ser hospedado; faça o commit pela janela principal do GitExtensions. |
| Comando mostra `✗ Failed (exit code N)` | Leia a saída bruta acima da linha de falha — é o erro literal do `git`/`git lfs`. |

## Plugins integrados

Outros plugins do GitExtensions do mesmo autor que combinam bem com o ZimerfeldLFS:

- **[GitExtensions.ZimerfeldTree](https://github.com/zimerfeld/GitExtensions.ZimerfeldTree)**
- **[GitExtensions.ZimerfeldCommitMsg](https://github.com/zimerfeld/GitExtensions.ZimerfeldCommitMsg)**

## Licença

Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (veja `LICENSE.txt`).
