// ZimerfeldLfsPlugin.cs — MEF plugin entry point for ZimerfeldLFS
// Licensed under CC BY-NC-ND 4.0 — Copyright (c) 2026 Zimerfeld

using System.ComponentModel.Composition;
using GitExtensions.Extensibility.Git;
using GitExtensions.Extensibility.Plugins;

namespace GitExtensions.ZimerfeldLFS;

/// <summary>
/// GitExtensions plugin that manages Git Large File Storage (LFS) in a persistent non-modal window,
/// guiding the user through Installation, the basic track/commit/push workflow, and cloning/pulling.
/// Registered via MEF so GitExtensions discovers it automatically at startup.
/// </summary>
[Export(typeof(IGitPlugin))]
public sealed class ZimerfeldLfsPlugin : GitPluginBase
{
    // Singleton form — one per GitExtensions session.
    private LfsForm? _form;

    // Current commands instance — updated by Register/Unregister as repos change. Used to open the
    // native commit/push dialogs against the repo selected in the window's own dropdown.
    private IGitUICommands? _commands;

    // ── Constructor ───────────────────────────────────────────────────────────

    public ZimerfeldLfsPlugin() : base(false)
    {
        // false = plugin has no configurable settings in the GitExtensions settings dialog.
        Name        = "ZimerfeldLFS";
        Description = "Gerencia o Git Large File Storage (LFS) em uma janela dedicada (ZimerfeldLFS). "
                    + "O Git LFS substitui arquivos grandes (áudio, vídeo, datasets) por ponteiros de "
                    + "texto leves no repositório, guardando o conteúdo real em um servidor separado — "
                    + "acelerando o clone e evitando o inchaço do repositório. A janela conduz o usuário "
                    + "em etapas: 1) Instalação (git lfs install), 2) Fluxo básico (track, commit, push) "
                    + "e 3) Clone & Pull (git lfs pull/fetch/checkout), com um diretório de trabalho "
                    + "escolhido de forma independente do GitExtensions.";
        Icon        = PluginIcon.ForMenu();
    }

    // ── IGitPlugin ────────────────────────────────────────────────────────────

    /// <summary>
    /// Called when the user clicks Plugins → ZimerfeldLFS.
    /// Opens the window (or brings it to the front if already open).
    /// </summary>
    public override bool Execute(GitUIEventArgs args)
    {
        string workDir = args.GitModule?.WorkingDir ?? string.Empty;
        DebugLog($"Execute    inst=#{_instanceId} dir='{workDir}'");

        if (_form is null || _form.IsDisposed)
        {
            // Notify GitExtensions to refresh its own UI after a commit/checkout.
            Action? notifyChanged = null;
            try { notifyChanged = () => args.GitUICommands?.RepoChangedNotifier?.Notify(); }
            catch { /* RepoChangedNotifier may not be available in every build */ }

            // Open the native commit dialog in-process, bound to the working dir selected in the
            // window's own cboRepo (not the GitExtensions host's active repository).
            // Returns null when unavailable (the form logs a fallback message).
            Func<IWin32Window, string, bool?> openCommit = (owner, workingDir) =>
            {
                try
                {
                    var commands = string.IsNullOrEmpty(workingDir)
                        ? _commands
                        : _commands?.WithWorkingDirectory(workingDir);
                    return commands?.StartCommitDialog(owner, string.Empty, false);
                }
                catch { return null; }
            };

            // Open the native push dialog in-process; returns true if push was completed.
            Func<IWin32Window, string, bool> openPush = (owner, workingDir) =>
            {
                if (_commands is null) return false;
                try
                {
                    var commands = string.IsNullOrEmpty(workingDir)
                        ? _commands
                        : _commands.WithWorkingDirectory(workingDir);
                    if (commands is null) return false;
                    commands.StartPushDialog(owner, pushOnShow: true, forceWithLease: false, pushCompleted: out bool pushCompleted);
                    return pushCompleted;
                }
                catch { return false; }
            };

            _form = new LfsForm(workDir, notifyChanged, openCommit, openPush);
            _form.FormClosed += (_, _) => _form = null;
        }
        else
        {
            _form.UpdateWorkingDir(workDir);
        }

        _form.Show();
        _form.BringToFront();

        // Return false: GitExtensions should NOT refresh its own UI (the window manages its own state).
        return false;
    }

    /// <summary>Captures the current commands instance so the window can open native dialogs.</summary>
    public override void Register(IGitUICommands commands)
    {
        base.Register(commands);
        _commands = commands;

        string regDir = commands.Module?.WorkingDir ?? string.Empty;
        DebugLog($"Register   inst=#{_instanceId} formOpen={_form is { IsDisposed: false }} dir='{regDir}'");

        // NOTE: ZimerfeldLFS is decoupled from the GitExtensions host UI. It subscribes to NO host
        // events; the window's repository is chosen exclusively through its own cboRepo. The host
        // working dir is used only once, as the pre-selected cboRepo value when the window opens.
    }

    /// <summary>Clears the captured commands instance.</summary>
    public override void Unregister(IGitUICommands commands)
    {
        DebugLog($"Unregister inst=#{_instanceId} dir='{commands.Module?.WorkingDir ?? string.Empty}'");

        _commands = null;
        base.Unregister(commands);
    }

    // ── Diagnostic logging ──────────────────────────────────────────────────────
    // Appends one timestamped line per plugin lifecycle event to a log file. Best-effort; never throws.
    private static int _instanceCounter;
    private readonly int _instanceId = System.Threading.Interlocked.Increment(ref _instanceCounter);

    private static readonly string DebugLogPath = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "GitExtensions", "ZimerfeldLFS.debug.log");

    internal static void DebugLog(string message)
    {
        try
        {
            System.IO.File.AppendAllText(DebugLogPath,
                $"{DateTime.Now:HH:mm:ss.fff}  {message}{Environment.NewLine}");
        }
        catch { /* logging must never break the plugin */ }
    }
}
