using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    internal sealed class GitDetails : IGitDetails {
        private const string CommitTypeRegexInclusive = @"^[a-zA-Z]+(?:\(.+\))?: ";
        private const string CommitTypeRegexExclusive = @"^[a-zA-Z]+";
        private const string BreakingChangeCommitTypeRegex = @"^[a-zA-Z]+(?:\(.+\))?!: ";
        private const string BreakingChangeFooterRegex = @"\n\nBREAKING CHANGE: ";

        public GitDetails(
            IGitTag? latestTag,
            IGitTag? latestPrereleaseTag,
            IEnumerable<Commit> latestCommits,
            Options options
        ) {
            LatestTag = latestTag;
            LatestPrereleaseTag = latestPrereleaseTag;
            LatestCommits = latestCommits;
            Options = options;
        }

        private Options Options { get; }

        public IGitTag? LatestTag { get; }
        public IGitTag? LatestPrereleaseTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }

        [Pure]
        public IGitTag? BumpTag() {
            ReleaseType releaseType = GetReleaseType();
            if (releaseType == ReleaseType.None)
                return null;

            return Options.Prerelease ? BumpPrereleaseTag(releaseType) : BumpReleaseTag(releaseType);
        }

        private IGitTag BumpPrereleaseTag(ReleaseType releaseType) {
            IVersion latestVersion = LatestTag?.Version ?? IVersion.First;
            if (LatestPrereleaseTag == null)
                return IGitTag.Create(IVersion.From($"{latestVersion}.{Options.Channel}.1"), Options.Prefix,
                    Options.Suffix);

            IVersion newVersion = latestVersion.Bump(releaseType);
            IVersion prereleaseVersion = LatestPrereleaseTag.Version;

            if (releaseType != ReleaseType.None && newVersion.Major == prereleaseVersion.Major &&
                newVersion.Minor == prereleaseVersion.Minor && newVersion.Patch == prereleaseVersion.Patch)
                return IGitTag.Create(prereleaseVersion.BumpPrerelease(), LatestPrereleaseTag.Prefix,
                    LatestPrereleaseTag.Suffix);

            return IGitTag.Create(prereleaseVersion.Bump(releaseType), LatestPrereleaseTag.Prefix,
                LatestPrereleaseTag.Suffix);
        }

        private IGitTag BumpReleaseTag(ReleaseType releaseType) {
            return LatestTag == null
                ? IGitTag.Create(IVersion.First, Options.Prefix, Options.Suffix)
                : IGitTag.Create(LatestTag.Version.Bump(releaseType), LatestTag.Prefix, LatestTag.Suffix);
        }

        private ReleaseType GetReleaseType() {
            ReleaseType currentType = ReleaseType.None;
            foreach (Commit commit in LatestCommits) {
                if (IsBreakingChange(commit)) {
                    currentType = ReleaseType.Major;
                    break;
                }

                string match = Regex.Match(commit.Message, CommitTypeRegexInclusive).Value;
                match = Regex.Match(match, CommitTypeRegexExclusive).Value;

                if (!ConventionalCommits.Map.TryGetValue(match, out ReleaseType type))
                    type = ReleaseType.None;

                if (type > currentType)
                    currentType = type;
            }

            return currentType;
        }

        private bool IsBreakingChange(Commit commit) {
            return Regex.IsMatch(commit.Message, BreakingChangeCommitTypeRegex) ||
                   Regex.IsMatch(commit.MessageFull, BreakingChangeFooterRegex);
        }
    }
}
