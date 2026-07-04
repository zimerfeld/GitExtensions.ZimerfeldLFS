// LfsService.cs — Git / Git-LFS operations for the ZimerfeldLFS plugin
// Licensed under CC BY-NC-ND 4.0 — Copyright (c) 2026 Zimerfeld

using System.Diagnostics;
using System.Xml.Linq;

namespace GitExtensions.ZimerfeldLFS;

/// <summary>Result of a git/git-lfs invocation: captured stdout, stderr and process exit code.</summary>
public readonly record struct GitResult(string StdOut, string StdErr, int ExitCode)
{
    /// <summary>True when the process exited with code 0.</summary>
    public bool Ok => ExitCode == 0;

    /// <summary>Combined stdout+stderr, trimmed — convenient for the output log.</summary>
    public string Combined
    {
        get
        {
            var text = (StdOut + (StdOut.Length > 0 && StdErr.Length > 0 ? Environment.NewLine : string.Empty) + StdErr).Trim();
            return text;
        }
    }
}

/// <summary>
/// Runs <c>git</c> and <c>git lfs</c> subprocesses against a working directory and parses their
/// output for <see cref="LfsForm"/>. The working directory is chosen independently of the
/// GitExtensions host via the window's own repository dropdown.
/// </summary>
public sealed class LfsService
{
    public string WorkingDir { get; set; }

    // Seam: the real implementation spawns a git process; tests inject a fake runner.
    private readonly Func<string, GitResult> _runGit;

    public LfsService(string workingDir) : this(workingDir, null) { }

    /// <summary>Test-only constructor: <paramref name="gitRunner"/> replaces the real git process runner.</summary>
    internal LfsService(string workingDir, Func<string, GitResult>? gitRunner)
    {
        WorkingDir = workingDir ?? string.Empty;
        _runGit = gitRunner ?? RunGitProcess;
    }

    // ── Internal runner ──────────────────────────────────────────────────────

    /// <summary>Runs <c>git <paramref name="arguments"/></c> and captures stdout, stderr and exit code.</summary>
    public GitResult RunGit(string arguments) => _runGit(arguments);

    private GitResult RunGitProcess(string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo("git", arguments)
            {
                WorkingDirectory       = WorkingDir,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                CreateNoWindow         = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                StandardErrorEncoding  = System.Text.Encoding.UTF8
            };

            using var proc = Process.Start(psi);
            if (proc is null) return new GitResult(string.Empty, "Failed to start git.", -1);
            string stdout = proc.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            return new GitResult(stdout, stderr, proc.ExitCode);
        }
        catch (Exception ex)
        {
            // git not on PATH, etc. — surface as a failed result rather than throwing.
            return new GitResult(string.Empty, ex.Message, -1);
        }
    }

    // ── Repository state ─────────────────────────────────────────────────────

    /// <summary>True when <see cref="WorkingDir"/> is inside a git work tree.</summary>
    public bool IsGitRepo()
    {
        if (string.IsNullOrEmpty(WorkingDir) || !Directory.Exists(WorkingDir)) return false;
        var r = RunGit("rev-parse --is-inside-work-tree");
        return r.Ok && r.StdOut.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Currently checked-out branch, or empty string.</summary>
    public string GetCurrentBranch()
    {
        var r = RunGit("rev-parse --abbrev-ref HEAD");
        return r.Ok ? r.StdOut.Trim() : string.Empty;
    }

    /// <summary>Number of pending changes (staged, unstaged and untracked) in the working tree.</summary>
    public int GetPendingChangesCount()
    {
        var r = RunGit("status --porcelain");
        if (!r.Ok) return 0;
        return SplitLines(r.StdOut).Count();
    }

    // ── Step 1 · Installation ────────────────────────────────────────────────

    /// <summary>Runs <c>git lfs version</c>; empty StdOut means Git LFS is not installed / not on PATH.</summary>
    public GitResult GetLfsVersion() => RunGit("lfs version");

    /// <summary>True when the Git LFS extension is available (the <c>git lfs</c> command resolves).</summary>
    public bool IsLfsAvailable()
    {
        var r = GetLfsVersion();
        return r.Ok && r.StdOut.TrimStart().StartsWith("git-lfs", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// True when <c>git lfs install</c> has configured the LFS filters for the current user
    /// (i.e. <c>filter.lfs.clean</c> is present in the global git config).
    /// </summary>
    public bool IsLfsInitializedForUser()
    {
        var r = RunGit("config --global --get filter.lfs.clean");
        return r.Ok && r.StdOut.Trim().Length > 0;
    }

    /// <summary>Runs <c>git lfs install</c> to initialize Git LFS for the user account.</summary>
    public GitResult LfsInstall() => RunGit("lfs install");

    // ── Step 2 · Basic workflow (track / commit / push) ──────────────────────

    /// <summary>
    /// Returns the glob patterns currently tracked by Git LFS (from <c>git lfs track</c>).
    /// Each entry is the raw pattern, e.g. <c>*.psd</c>.
    /// </summary>
    public List<string> GetTrackedPatterns()
    {
        var r = RunGit("lfs track");
        return r.Ok ? ParseTrackedPatterns(r.StdOut) : [];
    }

    /// <summary>Parses the raw output of <c>git lfs track</c> into the list of tracked glob patterns.</summary>
    internal static List<string> ParseTrackedPatterns(string stdout)
    {
        var patterns = new List<string>();
        foreach (var line in SplitLines(stdout))
        {
            var trimmed = line.Trim();
            // Output looks like:  "Listing tracked patterns"  then  "    *.psd (.gitattributes)"
            if (trimmed.Length == 0) continue;
            if (trimmed.StartsWith("Listing", StringComparison.OrdinalIgnoreCase)) continue;
            if (trimmed.StartsWith("Tracking", StringComparison.OrdinalIgnoreCase)) continue;

            // Strip the trailing " (.gitattributes)" source annotation if present.
            int paren = trimmed.IndexOf(" (", StringComparison.Ordinal);
            if (paren > 0) trimmed = trimmed[..paren].Trim();
            if (trimmed.Length > 0) patterns.Add(trimmed);
        }
        return patterns;
    }

    /// <summary>Tracks a glob pattern: <c>git lfs track "&lt;pattern&gt;"</c>.</summary>
    public GitResult TrackPattern(string pattern) => RunGit($"lfs track \"{pattern}\"");

    /// <summary>Stops tracking a glob pattern: <c>git lfs untrack "&lt;pattern&gt;"</c>.</summary>
    public GitResult UntrackPattern(string pattern) => RunGit($"lfs untrack \"{pattern}\"");

    /// <summary>Files currently managed by Git LFS in the work tree (<c>git lfs ls-files</c>).</summary>
    public List<string> GetLfsFiles()
    {
        var r = RunGit("lfs ls-files");
        return r.Ok ? ParseLfsFiles(r.StdOut) : [];
    }

    /// <summary>Parses the raw output of <c>git lfs ls-files</c> into non-empty file lines.</summary>
    // Each line: "<oid> <*|-> <path>" — keep the whole line; it is informative as-is.
    internal static List<string> ParseLfsFiles(string stdout) =>
        SplitLines(stdout).Select(l => l.TrimEnd()).Where(l => l.Length > 0).ToList();

    /// <summary>Stages the given paths (used to add <c>.gitattributes</c> after tracking).</summary>
    public GitResult Add(string pathspec) => RunGit($"add -- {pathspec}");

    /// <summary>Runs <c>git push</c> for the current branch (fallback when no host dialog is available).</summary>
    public GitResult Push() => RunGit("push");

    // ── Step 3 · Cloning & pulling ───────────────────────────────────────────

    /// <summary>Downloads LFS content for the current checkout: <c>git lfs pull</c>.</summary>
    public GitResult LfsPull() => RunGit("lfs pull");

    /// <summary>Prefetches all LFS objects for every ref: <c>git lfs fetch --all</c>.</summary>
    public GitResult LfsFetchAll() => RunGit("lfs fetch --all");

    /// <summary>Populates working-tree files from downloaded LFS objects: <c>git lfs checkout</c>.</summary>
    public GitResult LfsCheckout() => RunGit("lfs checkout");

    /// <summary>Shows the status of LFS objects in the work tree: <c>git lfs status</c>.</summary>
    public GitResult LfsStatus() => RunGit("lfs status");

    // ── Static helpers ───────────────────────────────────────────────────────

    private static IEnumerable<string> SplitLines(string s) =>
        s.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// Reads the GitExtensions repository history from its settings file so the window's own
    /// repository dropdown can be populated independently of the host UI.
    /// </summary>
    public static List<string> GetRepositoriesFromSettings()
    {
        var result = new List<string>();
        try
        {
            string settingsFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GitExtensions", "GitExtensions", "GitExtensions.settings");

            if (!File.Exists(settingsFile)) return result;

            var doc = XDocument.Load(settingsFile);

            // GitExtensions stores repository history under key "history" as an XML-encoded string.
            var historyValue = doc
                .Descendants("item")
                .FirstOrDefault(item =>
                    item.Element("key")?.Element("string")?.Value
                        .Equals("history", StringComparison.OrdinalIgnoreCase) == true)
                ?.Element("value")
                ?.Element("string")
                ?.Value;

            if (!string.IsNullOrWhiteSpace(historyValue))
            {
                var inner = XDocument.Parse(historyValue);
                foreach (var pathEl in inner.Descendants("Path"))
                {
                    var path = pathEl.Value?.Trim();
                    if (!string.IsNullOrEmpty(path))
                        result.Add(path);
                }

                if (result.Count > 0)
                    return result.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            }
        }
        catch { /* best-effort — an empty list is fine */ }

        return result.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }
}
