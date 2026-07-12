// LfsForm.cs — Main WinForms window for the ZimerfeldLFS plugin
// Licensed under CC BY-NC-ND 4.0 — Copyright (c) 2026 Zimerfeld

namespace GitExtensions.ZimerfeldLFS;

/// <summary>
/// Non-modal window that guides the user through the Git LFS workflow in three steps —
/// Installation, Basic Workflow (track / commit / push) and Cloning &amp; Pulling — against a
/// repository chosen through its own working-directory dropdown, independently of the
/// GitExtensions host UI.
/// </summary>
public sealed class LfsForm : Form
{
    // ── Services ─────────────────────────────────────────────────────────────
    private readonly LfsService _svc;
    private readonly Action? _notifyRepoChanged;
    /// <summary>Opens the native GitExtensions commit dialog in-process for the given working dir.
    /// Returns true = commits made, false = closed without committing, null = unavailable.</summary>
    private readonly Func<IWin32Window, string, bool?>? _openCommitDialog;
    /// <summary>Opens the native GitExtensions push dialog in-process for the given working dir.
    /// Returns true when a push completed.</summary>
    private readonly Func<IWin32Window, string, bool>? _openPushDialog;

    // ── Localization ─────────────────────────────────────────────────────────
    private Translator _t = I18n.Load("ZimerfeldLFS");

    // ── Top panel (independent working directory) ────────────────────────────
    private Panel    _topPanel  = null!;
    private Label    _lblWD     = null!;
    private ComboBox _cboRepo   = null!;
    private Label    _lblBranch = null!;

    // ── Steps (tabs) ─────────────────────────────────────────────────────────
    private TabControl _tabs = null!;
    private TabPage    _tabInstall  = null!;
    private TabPage    _tabWorkflow = null!;
    private TabPage    _tabClone    = null!;

    // Step 1 · Installation
    private Label  _lblInstallStatus = null!;
    private Label  _lblInstallHelp   = null!;
    private Button _btnCheckInstall  = null!;
    private Button _btnLfsInstall     = null!;

    // Step 2 · Basic workflow
    private Label   _lblTrackHint = null!;
    private TextBox _txtPattern   = null!;
    private Button  _btnTrack     = null!;
    private Label   _lblPatterns  = null!;
    private ListBox _lstPatterns  = null!;
    private Button  _btnUntrack   = null!;
    private Label   _lblLfsFiles  = null!;
    private ListBox _lstLfsFiles  = null!;
    private Button  _btnCommit    = null!;
    private Button  _btnPush      = null!;

    // Step 3 · Cloning & pulling
    private Label  _lblCloneHelp   = null!;
    private Button _btnLfsPull     = null!;
    private Button _btnLfsFetchAll = null!;
    private Button _btnLfsCheckout = null!;
    private Button _btnLfsStatus   = null!;

    // ── Output log ────────────────────────────────────────────────────────────
    private Panel   _logPanel = null!;
    private Label   _lblLog = null!;
    private TextBox _txtLog = null!;
    private Button  _btnClearLog = null!;

    // ── Bottom panel ──────────────────────────────────────────────────────────
    private Panel     _bottomPanel = null!;
    private Button    _btnClose    = null!;
    private CheckBox  _chkShowDebug = null!;
    private Label     _lblLanguage = null!;
    private ComboBox  _cboLanguage = null!;
    private bool      _suppressLangEvent;
    private LinkLabel _lnkAbout    = null!;

    // ── Status strip ────────────────────────────────────────────────────────
    private StatusStrip          _status    = null!;
    private ToolStripStatusLabel _statusLbl = null!;

    // ── Runtime state ───────────────────────────────────────────────────────
    private bool _busy;
    private readonly ToolTip _debugTip = new();

    private static readonly string UiSettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "GitExtensions", "ZimerfeldLFS.uisettings.json");

    // ─────────────────────────────────────────────────────────────────────────
    public LfsForm(string workingDir, Action? notifyRepoChanged = null,
        Func<IWin32Window, string, bool?>? openCommitDialog = null,
        Func<IWin32Window, string, bool>? openPushDialog = null)
    {
        _svc = new LfsService(workingDir);
        _notifyRepoChanged = notifyRepoChanged;
        _openCommitDialog  = openCommitDialog;
        _openPushDialog    = openPushDialog;

        InitializeComponent();
        LoadRepositories();   // combo population only — reads the settings XML, no git subprocess
        // The constructor does NO git work, so the window appears instantly. The first data probe
        // (LFS version, tracked patterns, files) runs behind the Shown event on a background thread.
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Called by the plugin when GitExtensions switches the active repository.</summary>
    public void UpdateWorkingDir(string newDir)
    {
        _svc.WorkingDir = newDir;
        if (!_cboRepo.Items.Contains(newDir))
            _cboRepo.Items.Add(newDir);
        _cboRepo.SelectedItem = newDir;
    }

    // ── Initialization ────────────────────────────────────────────────────────

    private void InitializeComponent()
    {
        SuspendLayout();

        Text            = _t["title"];
        Size            = new Size(720, 720 + SponsorBanner.PanelHeight);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;   // not user-resizable
        MaximizeBox     = false;
        MinimizeBox     = true;
        KeyPreview      = true;
        Font            = new Font("Segoe UI", 9f);
        Icon            = PluginIcon.ForForm();

        BuildTopPanel();
        BuildAboutLink();
        BuildTabs();
        BuildLogPanel();
        BuildBottomPanel();
        BuildStatusStrip();
        ApplyLanguage();

        // Dock order: added last = topmost for DockStyle.Top.
        // Visual order top→bottom: sponsor, topPanel, tabs (Fill), logPanel, bottomPanel, status.
        Controls.Add(_tabs);                            // Fill
        Controls.Add(_topPanel);                        // Top
        Controls.Add(SponsorBanner.Create(_lnkAbout));  // Top (topmost)
        Controls.Add(_logPanel);                        // Bottom
        Controls.Add(_bottomPanel);                     // Bottom
        Controls.Add(_status);                          // Bottom (lowest)

        CancelButton = _btnClose;

        Load  += (_, _) => ApplyControlTooltips(_chkShowDebug.Checked);
        Shown += (_, _) => _ = RefreshStateAsync();

        ResumeLayout(false);
        PerformLayout();
    }

    private void BuildTopPanel()
    {
        _topPanel = new Panel { Name = "topPanel", Dock = DockStyle.Top, Height = 82 };

        var table = new TableLayoutPanel
        {
            Name        = "tblTop",
            Dock        = DockStyle.Fill,
            ColumnCount = 1,
            RowCount    = 3,
            Padding     = new Padding(8, 12, 8, 4),
            Margin      = Padding.Empty
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _lblWD = new Label
        {
            Name     = "lblWD",
            Text     = _t["workingDirectory"],
            AutoSize = true,
            Font     = new Font(Font, FontStyle.Bold),
            Margin   = new Padding(0, 0, 0, 2)
        };

        _cboRepo = new ComboBox
        {
            Name          = "cboRepo",
            DropDownStyle = ComboBoxStyle.DropDownList,
            Sorted        = true,
            Dock          = DockStyle.Fill,
            Margin        = new Padding(0, 0, 0, 2)
        };
        _cboRepo.SelectedIndexChanged += CboRepo_SelectedIndexChanged;

        _lblBranch = new Label
        {
            Name     = "lblBranch",
            AutoSize = true,
            Text     = "Branch: ",
            Margin   = Padding.Empty
        };

        table.Controls.Add(_lblWD,     0, 0);
        table.Controls.Add(_cboRepo,   0, 1);
        table.Controls.Add(_lblBranch, 0, 2);

        _topPanel.Controls.Add(table);
    }

    private void BuildAboutLink()
    {
        _lnkAbout = new LinkLabel { Name = "lnkAbout", Text = _t["about"], AutoSize = true };
        _lnkAbout.LinkClicked += (_, _) => ShowAbout();
    }

    private void BuildTabs()
    {
        _tabs = new TabControl { Name = "tabs", Dock = DockStyle.Fill, Padding = new Point(12, 6) };

        _tabInstall  = new TabPage { Name = "tabInstall",  Text = _t["step1Title"], Padding = new Padding(10) };
        _tabWorkflow = new TabPage { Name = "tabWorkflow", Text = _t["step2Title"], Padding = new Padding(10) };
        _tabClone    = new TabPage { Name = "tabClone",    Text = _t["step3Title"], Padding = new Padding(10) };

        BuildInstallTab();
        BuildWorkflowTab();
        BuildCloneTab();

        _tabs.TabPages.AddRange([_tabInstall, _tabWorkflow, _tabClone]);
    }

    private void BuildInstallTab()
    {
        _lblInstallStatus = new Label
        {
            Name      = "lblInstallStatus",
            AutoSize  = false,
            Dock      = DockStyle.Top,
            Height    = 48,
            Font      = new Font(Font, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding   = new Padding(2, 0, 2, 0)
        };

        var buttons = new FlowLayoutPanel
        {
            Name = "installButtons", Dock = DockStyle.Top, Height = 40, WrapContents = false, AutoSize = false
        };
        _btnCheckInstall = new Button { Name = "btnCheckInstall", Text = _t["btnCheckInstall"], Width = 200, Height = 30 };
        _btnCheckInstall.Click += (_, _) => _ = RunAsync(_t["btnCheckInstall"], () => _svc.GetLfsVersion());
        _btnLfsInstall = new Button { Name = "btnLfsInstall", Text = _t["btnLfsInstall"], Width = 200, Height = 30, Margin = new Padding(8, 3, 3, 3) };
        _btnLfsInstall.Click += (_, _) => _ = RunAsync("git lfs install", () => _svc.LfsInstall());
        buttons.Controls.AddRange([_btnCheckInstall, _btnLfsInstall]);

        _lblInstallHelp = new Label
        {
            Name     = "lblInstallHelp",
            Dock     = DockStyle.Fill,
            AutoSize = false,
            Text     = _t["step1Help"],
            Padding  = new Padding(2, 8, 2, 2)
        };

        // Fill added first so Top-docked controls sit above it.
        _tabInstall.Controls.Add(_lblInstallHelp);
        _tabInstall.Controls.Add(buttons);
        _tabInstall.Controls.Add(_lblInstallStatus);
    }

    private void BuildWorkflowTab()
    {
        _lblTrackHint = new Label { Name = "lblTrackHint", Dock = DockStyle.Top, Height = 22, Text = _t["trackHint"], AutoSize = false };

        var trackRow = new Panel { Name = "trackRow", Dock = DockStyle.Top, Height = 34, Padding = new Padding(0, 2, 0, 4) };
        _btnTrack = new Button { Name = "btnTrack", Text = _t["btnTrack"], Dock = DockStyle.Right, Width = 120, Height = 26 };
        _btnTrack.Click += (_, _) => DoTrack();
        _txtPattern = new TextBox { Name = "txtPattern", Dock = DockStyle.Fill, PlaceholderText = _t["patternPlaceholder"] };
        _txtPattern.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; DoTrack(); } };
        trackRow.Controls.Add(_txtPattern);
        trackRow.Controls.Add(_btnTrack);

        _lblPatterns = new Label { Name = "lblPatterns", Dock = DockStyle.Top, Height = 20, Text = _t["patternsLabel"], Font = new Font(Font, FontStyle.Bold) };
        var patternsRow = new Panel { Name = "patternsRow", Dock = DockStyle.Top, Height = 96, Padding = new Padding(0, 0, 0, 4) };
        _btnUntrack = new Button { Name = "btnUntrack", Text = _t["btnUntrack"], Dock = DockStyle.Right, Width = 120, Height = 26 };
        _btnUntrack.Click += (_, _) => DoUntrack();
        _lstPatterns = new ListBox { Name = "lstPatterns", Dock = DockStyle.Fill, IntegralHeight = false };
        patternsRow.Controls.Add(_lstPatterns);
        patternsRow.Controls.Add(_btnUntrack);

        _lblLfsFiles = new Label { Name = "lblLfsFiles", Dock = DockStyle.Top, Height = 20, Text = _t.F("lfsFilesLabel", 0), Font = new Font(Font, FontStyle.Bold) };
        _lstLfsFiles = new ListBox { Name = "lstLfsFiles", Dock = DockStyle.Fill, IntegralHeight = false, HorizontalScrollbar = true };

        var commitRow = new FlowLayoutPanel { Name = "commitRow", Dock = DockStyle.Bottom, Height = 40, WrapContents = false, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 6, 0, 0) };
        _btnCommit = new Button { Name = "btnCommit", Text = _t["btnCommit"], Width = 150, Height = 28 };
        _btnCommit.Click += (_, _) => DoCommit();
        _btnPush = new Button { Name = "btnPush", Text = _t["btnPush"], Width = 150, Height = 28, Margin = new Padding(8, 3, 3, 3) };
        _btnPush.Click += (_, _) => DoPush();
        commitRow.Controls.AddRange([_btnCommit, _btnPush]);

        // Fill (_lstLfsFiles) added first, then bottom, then the top-docked controls in reverse
        // visual order so they stack correctly.
        _tabWorkflow.Controls.Add(_lstLfsFiles);   // Fill
        _tabWorkflow.Controls.Add(commitRow);      // Bottom
        _tabWorkflow.Controls.Add(_lblLfsFiles);   // Top
        _tabWorkflow.Controls.Add(patternsRow);    // Top
        _tabWorkflow.Controls.Add(_lblPatterns);   // Top
        _tabWorkflow.Controls.Add(trackRow);       // Top
        _tabWorkflow.Controls.Add(_lblTrackHint);  // Top
    }

    private void BuildCloneTab()
    {
        _lblCloneHelp = new Label { Name = "lblCloneHelp", Dock = DockStyle.Top, Height = 150, Text = _t["step3Help"], AutoSize = false, Padding = new Padding(2) };

        var buttons = new FlowLayoutPanel { Name = "cloneButtons", Dock = DockStyle.Top, Height = 80, WrapContents = true, AutoSize = false };
        _btnLfsPull     = new Button { Name = "btnLfsPull",     Text = _t["btnLfsPull"],     Width = 200, Height = 30, Margin = new Padding(3, 3, 8, 8) };
        _btnLfsFetchAll = new Button { Name = "btnLfsFetchAll", Text = _t["btnLfsFetchAll"], Width = 200, Height = 30, Margin = new Padding(3, 3, 8, 8) };
        _btnLfsCheckout = new Button { Name = "btnLfsCheckout", Text = _t["btnLfsCheckout"], Width = 200, Height = 30, Margin = new Padding(3, 3, 8, 8) };
        _btnLfsStatus   = new Button { Name = "btnLfsStatus",   Text = _t["btnLfsStatus"],   Width = 200, Height = 30, Margin = new Padding(3, 3, 8, 8) };
        _btnLfsPull.Click     += (_, _) => _ = RunAsync("git lfs pull",      () => _svc.LfsPull());
        _btnLfsFetchAll.Click += (_, _) => _ = RunAsync("git lfs fetch --all", () => _svc.LfsFetchAll());
        _btnLfsCheckout.Click += (_, _) => _ = RunAsync("git lfs checkout",  () => _svc.LfsCheckout());
        _btnLfsStatus.Click   += (_, _) => _ = RunAsync("git lfs status",    () => _svc.LfsStatus(), refreshAfter: false);
        buttons.Controls.AddRange([_btnLfsPull, _btnLfsFetchAll, _btnLfsCheckout, _btnLfsStatus]);

        _tabClone.Controls.Add(buttons);      // Top
        _tabClone.Controls.Add(_lblCloneHelp);// Top (added after → sits above buttons)
    }

    private void BuildLogPanel()
    {
        var panel = new Panel { Name = "logPanel", Dock = DockStyle.Bottom, Height = 150 };

        var header = new Panel { Name = "logHeader", Dock = DockStyle.Top, Height = 22 };
        _lblLog = new Label { Name = "lblLog", Text = _t["logLabel"], Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(4, 4, 0, 0), Font = new Font(Font, FontStyle.Bold) };
        _btnClearLog = new Button { Name = "btnClearLog", Text = _t["btnClearLog"], Dock = DockStyle.Right, Width = 90, Height = 22 };
        _btnClearLog.Click += (_, _) => _txtLog.Clear();
        header.Controls.Add(_lblLog);
        header.Controls.Add(_btnClearLog);

        _txtLog = new TextBox
        {
            Name        = "txtLog",
            Dock        = DockStyle.Fill,
            Multiline   = true,
            ReadOnly    = true,
            ScrollBars  = ScrollBars.Both,
            WordWrap    = false,
            BackColor   = Color.FromArgb(30, 30, 30),
            ForeColor   = Color.Gainsboro,
            Font        = new Font("Consolas", 9f)
        };

        panel.Controls.Add(_txtLog);
        panel.Controls.Add(header);
        _logPanel = panel;
    }

    private void BuildBottomPanel()
    {
        _btnClose = new Button
        {
            Name         = "btnClose",
            Text         = _t["close"],
            Width        = 80,
            Height       = 26,
            DialogResult = DialogResult.Cancel
        };
        _btnClose.Click += (_, _) => Close();

        _chkShowDebug = new CheckBox
        {
            Name     = "chkShowDebug",
            Text     = _t["showDebug"],
            AutoSize = true,
            Checked  = LoadShowControlIds()
        };
        _chkShowDebug.CheckedChanged += (_, _) =>
        {
            SaveUiSettings();
            ApplyControlTooltips(_chkShowDebug.Checked);
        };

        _lblLanguage = new Label { Name = "lblLanguage", Text = _t["language"], AutoSize = true, TextAlign = ContentAlignment.MiddleRight };
        _cboLanguage = new ComboBox { Name = "cboLanguage", DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
        _cboLanguage.SelectedIndexChanged += OnLanguageChanged;

        _bottomPanel = new Panel { Name = "bottomPanel", Dock = DockStyle.Bottom, Height = 36 };
        _bottomPanel.Controls.Add(_btnClose);
        _bottomPanel.Controls.Add(_chkShowDebug);
        _bottomPanel.Controls.Add(_lblLanguage);
        _bottomPanel.Controls.Add(_cboLanguage);

        _bottomPanel.Layout += (_, _) =>
        {
            int cy = (_bottomPanel.Height - _btnClose.Height) / 2;
            _btnClose.Location     = new Point((_bottomPanel.Width - _btnClose.Width) / 2, cy);
            _chkShowDebug.Location = new Point(8, (_bottomPanel.Height - _chkShowDebug.Height) / 2);
            _cboLanguage.Location  = new Point(_bottomPanel.Width - _cboLanguage.Width - 8, (_bottomPanel.Height - _cboLanguage.Height) / 2);
            _lblLanguage.Location  = new Point(_cboLanguage.Left - _lblLanguage.Width - 6, (_bottomPanel.Height - _lblLanguage.Height) / 2);
        };
    }

    private void BuildStatusStrip()
    {
        _status = new StatusStrip { SizingGrip = false, Renderer = new NoGripRenderer() };
        _statusLbl = new ToolStripStatusLabel { Text = _t["statusReady"], Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        _status.Items.Add(_statusLbl);
    }

    // Suppresses the StatusStrip sizing-grip image that SizingGrip=false alone does not remove.
    private sealed class NoGripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e) { }
    }

    // ── Repository dropdown ─────────────────────────────────────────────────────

    private void LoadRepositories()
    {
        _cboRepo.Items.Clear();
        var repos = LfsService.GetRepositoriesFromSettings();

        if (!string.IsNullOrEmpty(_svc.WorkingDir) &&
            !repos.Contains(_svc.WorkingDir, StringComparer.OrdinalIgnoreCase))
        {
            repos.Insert(0, _svc.WorkingDir);
        }

        foreach (var r in repos) _cboRepo.Items.Add(r);

        if (_svc.WorkingDir.Length > 0)
            _cboRepo.SelectedItem = _svc.WorkingDir;
        else if (_cboRepo.Items.Count > 0)
            _cboRepo.SelectedIndex = 0;
    }

    private void CboRepo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cboRepo.SelectedItem is string dir && dir != _svc.WorkingDir)
        {
            _svc.WorkingDir = dir;
            _ = RefreshStateAsync();
        }
    }

    // ── Language selection ──────────────────────────────────────────────────────

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        if (_suppressLangEvent) return;
        var lang = _cboLanguage.SelectedIndex switch
        {
            1 => AppLanguage.English,
            2 => AppLanguage.Portuguese,
            3 => AppLanguage.Spanish,
            _ => AppLanguage.Automatic,
        };
        I18n.SetLanguage(lang);
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        _t = I18n.Load("ZimerfeldLFS");

        Text            = _t["title"];
        _lblWD.Text     = _t["workingDirectory"];
        _lnkAbout.Text  = _t["about"];

        _tabInstall.Text  = _t["step1Title"];
        _tabWorkflow.Text = _t["step2Title"];
        _tabClone.Text    = _t["step3Title"];

        // Step 1
        _btnCheckInstall.Text = _t["btnCheckInstall"];
        _btnLfsInstall.Text   = _t["btnLfsInstall"];
        _lblInstallHelp.Text  = _t["step1Help"];

        // Step 2
        _lblTrackHint.Text          = _t["trackHint"];
        _txtPattern.PlaceholderText = _t["patternPlaceholder"];
        _btnTrack.Text              = _t["btnTrack"];
        _lblPatterns.Text           = _t["patternsLabel"];
        _btnUntrack.Text            = _t["btnUntrack"];
        _lblLfsFiles.Text           = _t.F("lfsFilesLabel", _lstLfsFiles.Items.Count);
        _btnCommit.Text             = _t["btnCommit"];
        _btnPush.Text               = _t["btnPush"];

        // Step 3
        _lblCloneHelp.Text     = _t["step3Help"];
        _btnLfsPull.Text       = _t["btnLfsPull"];
        _btnLfsFetchAll.Text   = _t["btnLfsFetchAll"];
        _btnLfsCheckout.Text   = _t["btnLfsCheckout"];
        _btnLfsStatus.Text     = _t["btnLfsStatus"];

        // Log + bottom
        _lblLog.Text        = _t["logLabel"];
        _btnClearLog.Text   = _t["btnClearLog"];
        _btnClose.Text      = _t["close"];
        _chkShowDebug.Text  = _t["showDebug"];
        _lblLanguage.Text   = _t["language"];

        PopulateLanguageCombo();
    }

    private void PopulateLanguageCombo()
    {
        _suppressLangEvent = true;
        int sel = _cboLanguage.SelectedIndex >= 0 ? _cboLanguage.SelectedIndex : (int)I18n.Current;
        _cboLanguage.Items.Clear();
        _cboLanguage.Items.AddRange([_t["langAutomatic"], _t["langEnglish"], _t["langPortuguese"], _t["langSpanish"]]);
        _cboLanguage.SelectedIndex = sel;
        _suppressLangEvent = false;
    }

    // ── Step 2 actions ──────────────────────────────────────────────────────────

    private void DoTrack()
    {
        var pattern = _txtPattern.Text.Trim();
        if (pattern.Length == 0)
        {
            MessageBox.Show(this, _t["errEmptyPattern"], _t["title"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        _txtPattern.Clear();
        _ = RunAsync($"git lfs track \"{pattern}\"", () =>
        {
            var r = _svc.TrackPattern(pattern);
            // Tracking updates .gitattributes — stage it so the change is ready to commit (per the workflow).
            if (r.Ok) _svc.Add(".gitattributes");
            return r;
        });
    }

    private void DoUntrack()
    {
        if (_lstPatterns.SelectedItem is not string pattern)
        {
            MessageBox.Show(this, _t["errNoPatternSelected"], _t["title"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        _ = RunAsync($"git lfs untrack \"{pattern}\"", () =>
        {
            var r = _svc.UntrackPattern(pattern);
            if (r.Ok) _svc.Add(".gitattributes");
            return r;
        });
    }

    private void DoCommit()
    {
        var dir = _svc.WorkingDir;
        // Prefer the native in-process commit dialog so all commit plugins load; fall back to a
        // separate GitExtensions process, and finally to a message if neither is available.
        bool? result = _openCommitDialog?.Invoke(this, dir);
        if (result is null)
        {
            Log(_t["logCommitUnavailable"]);
        }
        else
        {
            Log(result == true ? _t["logCommitDone"] : _t["logCommitClosed"]);
            _notifyRepoChanged?.Invoke();
        }
        _ = RefreshStateAsync();
    }

    private void DoPush()
    {
        var dir = _svc.WorkingDir;
        bool pushed = _openPushDialog?.Invoke(this, dir) ?? false;
        if (pushed)
        {
            Log(_t["logPushDone"]);
            _ = RefreshStateAsync();
            return;
        }
        // No host push dialog available (or it was cancelled): offer the plain `git push` fallback.
        Log(_t["logPushFallback"]);
        _ = RunAsync("git push", () => _svc.Push());
    }

    // ── Command runner ──────────────────────────────────────────────────────────

    /// <summary>
    /// Runs a git/git-lfs operation on a background thread, logging the command and its output,
    /// then (optionally) refreshes the window state. Concurrent calls are ignored while one runs.
    /// </summary>
    private async Task RunAsync(string title, Func<GitResult> op, bool refreshAfter = true)
    {
        if (_busy) return;
        if (!EnsureRepoSelected()) return;

        SetBusy(true, title);
        Log($"$ {title}");
        GitResult result;
        try { result = await Task.Run(op); }
        catch (Exception ex) { result = new GitResult(string.Empty, ex.Message, -1); }

        if (IsDisposed) return;

        if (result.Combined.Length > 0) Log(result.Combined);
        Log(result.Ok ? _t["logDone"] : _t.F("logFailed", result.ExitCode));
        SetBusy(false);

        if (refreshAfter) await RefreshStateAsync();
    }

    private bool EnsureRepoSelected()
    {
        if (_svc.WorkingDir.Length > 0 && Directory.Exists(_svc.WorkingDir)) return true;
        MessageBox.Show(this, _t["errNoRepo"], _t["title"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void SetBusy(bool busy, string? title = null)
    {
        _busy = busy;
        _topPanel.Enabled = !busy;
        _tabs.Enabled     = !busy;
        _statusLbl.Text   = busy ? _t.F("statusRunning", title) : _t["statusReady"];
        Cursor            = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    // ── State refresh ───────────────────────────────────────────────────────────

    private sealed record StateSnapshot(
        bool IsRepo, string Branch, string LfsVersion, bool LfsAvailable, bool LfsInitialized,
        List<string> Patterns, List<string> Files);

    /// <summary>
    /// Probes the selected repository on a background thread (git-lfs version, tracked patterns,
    /// LFS files, current branch) and applies the result to the UI.
    /// </summary>
    private async Task RefreshStateAsync()
    {
        if (IsDisposed) return;
        _statusLbl.Text = _t["statusRefreshing"];

        StateSnapshot snap;
        try
        {
            snap = await Task.Run(() =>
            {
                bool isRepo = _svc.IsGitRepo();
                string branch = isRepo ? _svc.GetCurrentBranch() : string.Empty;
                var ver = _svc.GetLfsVersion();
                bool avail = _svc.IsLfsAvailable();
                bool init = avail && _svc.IsLfsInitializedForUser();
                var patterns = (isRepo && avail) ? _svc.GetTrackedPatterns() : [];
                var files    = (isRepo && avail) ? _svc.GetLfsFiles()        : [];
                return new StateSnapshot(isRepo, branch,
                    ver.Ok ? ver.StdOut.Trim() : string.Empty, avail, init, patterns, files);
            });
        }
        catch { return; }

        if (IsDisposed) return;
        ApplyState(snap);
        _statusLbl.Text = _t["statusReady"];
    }

    private void ApplyState(StateSnapshot s)
    {
        // Branch label
        _lblBranch.Text = s.IsRepo
            ? _t.F("branchLabel", string.IsNullOrEmpty(s.Branch) ? _t["branchNone"] : s.Branch)
            : _t["notARepo"];

        // Step 1 · installation status
        if (!s.LfsAvailable)
        {
            _lblInstallStatus.Text      = _t["installMissing"];
            _lblInstallStatus.ForeColor = Color.DarkRed;
            _btnLfsInstall.Enabled      = false;
        }
        else
        {
            var ver = s.LfsVersion.Length > 0 ? s.LfsVersion : "git-lfs";
            _lblInstallStatus.Text = s.LfsInitialized
                ? _t.F("installReady", ver)
                : _t.F("installAvailable", ver);
            _lblInstallStatus.ForeColor = s.LfsInitialized ? Color.DarkGreen : Color.DarkGoldenrod;
            _btnLfsInstall.Enabled      = true;
        }

        // Step 2 · patterns + files
        _lstPatterns.BeginUpdate();
        _lstPatterns.Items.Clear();
        foreach (var p in s.Patterns) _lstPatterns.Items.Add(p);
        _lstPatterns.EndUpdate();

        _lstLfsFiles.BeginUpdate();
        _lstLfsFiles.Items.Clear();
        foreach (var f in s.Files) _lstLfsFiles.Items.Add(f);
        _lstLfsFiles.EndUpdate();
        _lblLfsFiles.Text = _t.F("lfsFilesLabel", s.Files.Count);

        bool workflowEnabled = s.IsRepo && s.LfsAvailable;
        _btnTrack.Enabled    = workflowEnabled;
        _btnUntrack.Enabled  = workflowEnabled;
        _btnCommit.Enabled   = s.IsRepo;
        _btnPush.Enabled     = s.IsRepo;
        _btnLfsPull.Enabled     = workflowEnabled;
        _btnLfsFetchAll.Enabled = workflowEnabled;
        _btnLfsCheckout.Enabled = workflowEnabled;
        _btnLfsStatus.Enabled   = workflowEnabled;
    }

    // ── Output log ──────────────────────────────────────────────────────────────

    private void Log(string message)
    {
        if (IsDisposed) return;
        void Append()
        {
            foreach (var line in message.Split('\n'))
                _txtLog.AppendText($"{line.TrimEnd('\r')}{Environment.NewLine}");
            _txtLog.SelectionStart = _txtLog.TextLength;
            _txtLog.ScrollToCaret();
        }
        if (InvokeRequired) BeginInvoke(Append); else Append();
    }

    // ── About ─────────────────────────────────────────────────────────────────

    private void ShowAbout() =>
        MessageBox.Show(this, _t["aboutText"], _t["about"], MessageBoxButtons.OK, MessageBoxIcon.Information);

    // ── Debug (control-id tooltips) ─────────────────────────────────────────────

    /// <summary>When enabled, shows each control's Name as a tooltip — the ZimerfeldTree debug aid.</summary>
    private void ApplyControlTooltips(bool show)
    {
        void Walk(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                _debugTip.SetToolTip(c, show && !string.IsNullOrEmpty(c.Name) ? c.Name : string.Empty);
                Walk(c);
            }
        }
        Walk(this);
    }

    // ── UI settings persistence ─────────────────────────────────────────────────

    private static bool LoadShowControlIds()
    {
        try
        {
            if (!File.Exists(UiSettingsPath)) return false;
            return File.ReadAllText(UiSettingsPath).Contains("\"showControlIds\":true");
        }
        catch { return false; }
    }

    private void SaveUiSettings()
    {
        try
        {
            string dir = Path.GetDirectoryName(UiSettingsPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(UiSettingsPath,
                $"{{\"showControlIds\":{(_chkShowDebug.Checked ? "true" : "false")}}}");
        }
        catch { /* best-effort */ }
    }
}
