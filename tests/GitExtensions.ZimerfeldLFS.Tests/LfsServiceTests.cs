// LfsServiceTests.cs — unit tests for the pure parsing and command-construction logic of LfsService.
// The real git process is replaced by a fake runner so these tests never touch git or the network.

using GitExtensions.ZimerfeldLFS;
using Xunit;

namespace GitExtensions.ZimerfeldLFS.Tests;

public class GitResultTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(-1, false)]
    [InlineData(2, false)]
    public void Ok_IsTrue_OnlyForExitCodeZero(int exitCode, bool expected)
    {
        var result = new GitResult("out", "err", exitCode);
        Assert.Equal(expected, result.Ok);
    }

    [Fact]
    public void Combined_JoinsStdOutAndStdErr_WithSingleNewline()
    {
        var result = new GitResult("hello", "world", 0);
        Assert.Equal("hello" + System.Environment.NewLine + "world", result.Combined);
    }

    [Fact]
    public void Combined_WithOnlyStdOut_HasNoLeadingOrTrailingNewline()
    {
        var result = new GitResult("just stdout", "", 0);
        Assert.Equal("just stdout", result.Combined);
    }

    [Fact]
    public void Combined_WithOnlyStdErr_ReturnsStdErrTrimmed()
    {
        var result = new GitResult("", "  boom  ", 1);
        Assert.Equal("boom", result.Combined);
    }

    [Fact]
    public void Combined_WithBothEmpty_IsEmptyString()
    {
        var result = new GitResult("", "", 0);
        Assert.Equal(string.Empty, result.Combined);
    }
}

public class ParseTrackedPatternsTests
{
    [Fact]
    public void StripsHeaderLine_AndGitattributesAnnotation()
    {
        // Typical `git lfs track` output.
        string stdout = string.Join('\n',
            "Listing tracked patterns",
            "    *.psd (.gitattributes)",
            "    *.mp4 (.gitattributes)");

        var patterns = LfsService.ParseTrackedPatterns(stdout);

        Assert.Equal(new[] { "*.psd", "*.mp4" }, patterns);
    }

    [Fact]
    public void SkipsTrackingHeader_Variant()
    {
        string stdout = string.Join('\n',
            "Tracking \"*.zip\"",
            "    *.zip (.gitattributes)");

        var patterns = LfsService.ParseTrackedPatterns(stdout);

        Assert.Equal(new[] { "*.zip" }, patterns);
    }

    [Fact]
    public void HandlesPatternWithoutAnnotation()
    {
        string stdout = "Listing tracked patterns\n    *.bin";
        var patterns = LfsService.ParseTrackedPatterns(stdout);
        Assert.Equal(new[] { "*.bin" }, patterns);
    }

    [Fact]
    public void EmptyOutput_ReturnsEmptyList()
    {
        Assert.Empty(LfsService.ParseTrackedPatterns(""));
        Assert.Empty(LfsService.ParseTrackedPatterns("Listing tracked patterns"));
    }
}

public class ParseLfsFilesTests
{
    [Fact]
    public void KeepsNonEmptyLines_Trimmed()
    {
        string stdout = string.Join('\n',
            "a1b2c3 * assets/logo.psd",
            "d4e5f6 - video/intro.mp4",
            "");

        var files = LfsService.ParseLfsFiles(stdout);

        Assert.Equal(2, files.Count);
        Assert.Equal("a1b2c3 * assets/logo.psd", files[0]);
        Assert.Equal("d4e5f6 - video/intro.mp4", files[1]);
    }

    [Fact]
    public void EmptyOutput_ReturnsEmptyList()
    {
        Assert.Empty(LfsService.ParseLfsFiles(""));
        Assert.Empty(LfsService.ParseLfsFiles("\n\n"));
    }
}

/// <summary>
/// Verifies the exact git argument strings each method builds, using a fake runner that
/// records the last invocation. No real git process is spawned.
/// </summary>
public class CommandConstructionTests
{
    private static (LfsService svc, System.Collections.Generic.List<string> calls) MakeService(
        System.Func<string, GitResult>? map = null)
    {
        var calls = new System.Collections.Generic.List<string>();
        var svc = new LfsService("C:\\repo", args =>
        {
            calls.Add(args);
            return map?.Invoke(args) ?? new GitResult(string.Empty, string.Empty, 0);
        });
        return (svc, calls);
    }

    [Fact]
    public void TrackPattern_QuotesTheGlob()
    {
        var (svc, calls) = MakeService();
        svc.TrackPattern("*.psd");
        Assert.Equal("lfs track \"*.psd\"", calls[^1]);
    }

    [Fact]
    public void UntrackPattern_QuotesTheGlob()
    {
        var (svc, calls) = MakeService();
        svc.UntrackPattern("*.mp4");
        Assert.Equal("lfs untrack \"*.mp4\"", calls[^1]);
    }

    [Fact]
    public void Add_PassesPathspec()
    {
        var (svc, calls) = MakeService();
        svc.Add(".gitattributes");
        Assert.Equal("add -- .gitattributes", calls[^1]);
    }

    [Theory]
    [InlineData("lfs pull")]
    [InlineData("lfs fetch --all")]
    [InlineData("lfs checkout")]
    [InlineData("lfs status")]
    [InlineData("lfs version")]
    [InlineData("lfs install")]
    [InlineData("push")]
    public void SimpleCommands_MapToExpectedArguments(string expected)
    {
        var (svc, calls) = MakeService();
        switch (expected)
        {
            case "lfs pull": svc.LfsPull(); break;
            case "lfs fetch --all": svc.LfsFetchAll(); break;
            case "lfs checkout": svc.LfsCheckout(); break;
            case "lfs status": svc.LfsStatus(); break;
            case "lfs version": svc.GetLfsVersion(); break;
            case "lfs install": svc.LfsInstall(); break;
            case "push": svc.Push(); break;
        }
        Assert.Equal(expected, calls[^1]);
    }
}

/// <summary>State-query methods interpret the fake runner's output correctly.</summary>
public class StateQueryTests
{
    private static LfsService WithRunner(System.Func<string, GitResult> runner) =>
        new("C:\\repo", runner);

    [Fact]
    public void IsLfsAvailable_TrueWhenStdOutStartsWithGitLfs()
    {
        var svc = WithRunner(_ => new GitResult("git-lfs/3.7.1 (GitHub; windows amd64)", "", 0));
        Assert.True(svc.IsLfsAvailable());
    }

    [Fact]
    public void IsLfsAvailable_FalseWhenExitNonZero()
    {
        var svc = WithRunner(_ => new GitResult("git-lfs/3.7.1", "", 1));
        Assert.False(svc.IsLfsAvailable());
    }

    [Fact]
    public void IsLfsAvailable_FalseWhenStdOutIsNotGitLfs()
    {
        var svc = WithRunner(_ => new GitResult("git version 2.45", "", 0));
        Assert.False(svc.IsLfsAvailable());
    }

    [Fact]
    public void IsLfsInitializedForUser_TrueWhenCleanFilterConfigured()
    {
        var svc = WithRunner(_ => new GitResult("git-lfs smudge -- %f", "", 0));
        Assert.True(svc.IsLfsInitializedForUser());
    }

    [Fact]
    public void IsLfsInitializedForUser_FalseWhenConfigMissing()
    {
        // `git config --get` on a missing key exits 1 with empty stdout.
        var svc = WithRunner(_ => new GitResult("", "", 1));
        Assert.False(svc.IsLfsInitializedForUser());
    }

    [Fact]
    public void GetCurrentBranch_ReturnsTrimmedBranchName()
    {
        var svc = WithRunner(_ => new GitResult("develop\n", "", 0));
        Assert.Equal("develop", svc.GetCurrentBranch());
    }

    [Fact]
    public void GetCurrentBranch_ReturnsEmptyOnFailure()
    {
        var svc = WithRunner(_ => new GitResult("", "fatal: not a git repository", 128));
        Assert.Equal(string.Empty, svc.GetCurrentBranch());
    }

    [Fact]
    public void GetPendingChangesCount_CountsPorcelainLines()
    {
        var svc = WithRunner(_ => new GitResult(" M file1\n?? file2\nA  file3\n", "", 0));
        Assert.Equal(3, svc.GetPendingChangesCount());
    }

    [Fact]
    public void GetPendingChangesCount_ZeroWhenClean()
    {
        var svc = WithRunner(_ => new GitResult("", "", 0));
        Assert.Equal(0, svc.GetPendingChangesCount());
    }

    [Fact]
    public void GetTrackedPatterns_EmptyWhenCommandFails()
    {
        var svc = WithRunner(_ => new GitResult("Listing tracked patterns\n    *.psd", "", 1));
        Assert.Empty(svc.GetTrackedPatterns());
    }

    [Fact]
    public void GetLfsFiles_ParsesOutputWhenOk()
    {
        var svc = WithRunner(_ => new GitResult("oid * a.psd\noid - b.mp4", "", 0));
        Assert.Equal(2, svc.GetLfsFiles().Count);
    }

    [Fact]
    public void IsGitRepo_FalseWhenWorkingDirIsEmpty()
    {
        // Empty working dir short-circuits before any git call.
        var svc = new LfsService("", _ => new GitResult("true", "", 0));
        Assert.False(svc.IsGitRepo());
    }
}
