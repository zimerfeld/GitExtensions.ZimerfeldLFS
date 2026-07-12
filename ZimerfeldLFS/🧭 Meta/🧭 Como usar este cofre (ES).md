---
tipo: meta
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-06-01
tags: [meta, protocolo]
---

# 🧭 Cómo usar este cofre (protocolo de Claude)

> 🇧🇷 Lea esta página en portugués → [[🧭 Como usar este cofre]]

> [!important] Protocolo de memoria
> Al **inicio** de cada sesión, leer: [[🏠 Home (ES)|🏠 Home]] (mapa por prioridad), [[📌 Backlog (ES)|📌 Backlog]] (punto de retomada), [[🔑 Fatos-Chave (ES)|🔑 Hechos Clave]] y la nota madre [[📦 GitExtensions.ZimerfeldLFS]].
> Al **final** de cada sesión, actualizar [[📌 Backlog (ES)|📌 Backlog]] y las notas afectadas (contenido + `atualizado:` del frontmatter).

## ✍️ Cuándo grabar memoria
| Situación | Dónde grabar |
|----------|-------------|
| Archivo fuente de alto impacto | `🔑 Arquivos-Chave/` |
| Subsistema/servicio reutilizado | `🧩 Sistemas/` |
| Se mapeó un flujo de uso | `🔀 Fluxos/` |
| Cómo ejecutar/publicar (runbook) | `🚀 Operação/` |
| Se tomó una decisión de arquitectura | `⚖️ Decisões/` |
| Concepto o patrón reutilizable | `📚 Conhecimento/` |
| Objetivo, stack, monetización, adopción | `💼 Negócio/` |
| Hechos clave, contexto de Renato, herramientas, inbox | `🧭 Meta/` |
| Punto de retomada / próximos pasos | `📌 Backlog.md` (raíz) |

## 🔗 Reglas de escritura
1. **Frontmatter estándar** (`tipo`, `projeto`, `lang`, `atualizado`) en toda nota — ver [[🏠 Home (ES)|🏠 Home]].
2. **Par bilingüe:** cada nota PT `<emoji> Nombre.md` tiene su par `<emoji> Nombre (EN).md` (traducción íntegra).
3. **Emoji + espacio** al comienzo de toda carpeta y todo archivo, y en los títulos `#`/`##`/`###`.
4. **Interconectar** con `[[wikilinks]]` — las notas PT enlazan nombres PT; las notas EN enlazan los pares `(EN)`.
5. **Atomicidad**: una idea por nota cuando sea posible. **Fechas en ISO** `AAAA-MM-DD`.
6. Usar **callouts** (`> [!note]`, `> [!warning]`) para destacar.

> [!note] Ordenación por prioridad
> El orden de las carpetas en el explorador está definido por [[sortspec]] (plugin **custom-sort**): impacto → reutilización → uso → operación. No borrar.

## 🧩 Plugins recomendados (opcionales)
- **Custom File Explorer sorting** (`custom-sort`) — **ya instalado**; aplica el orden de [[sortspec]].
- **Dataview** — listas dinámicas (opcional).
- **Templater** — plantillas avanzadas (opcional).
Sin los opcionales, el cofre funciona normalmente.

## 🔗 Relacionado
- [[🏠 Home (ES)|🏠 Home]] · [[📌 Backlog (ES)|📌 Backlog]] · [[🔑 Fatos-Chave (ES)|🔑 Hechos Clave]]
