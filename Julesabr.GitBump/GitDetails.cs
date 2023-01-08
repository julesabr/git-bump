using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public sealed class GitDetails : IGitDetails {
        private const string CommitTypeRegexInclusive = @"^[a-zA-Z]+(?:\(.+\))?: ";
        private const string CommitTypeRegexExclusive = @"^[a-zA-Z]+";
        private const string BreakingChangeCommitTypeRegex = @"^[a-zA-Z]+(?:\(.+\))?!: ";
        private const string BreakingChangeFooterRegex = @"\n\nBREAKING CHANGE: ";

        public GitDetails(
            IGitTag latestTag,
            IGitTag latestPrereleaseTag,
            IEnumerable<Commit> latestCommits,
            Options options
        ) {
            LatestTag = latestTag;
            LatestPrereleaseTag = latestPrereleaseTag;
            LatestCommits = latestCommits;
            Options = options;
        }

        private Options Options { get; }

        public IGitTag LatestTag { get; }
        public IGitTag LatestPrereleaseTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }

        [Pure]
        public IGitTag? BumpTag() {
            ReleaseType releaseType = GetReleaseType();
            if (releaseType == ReleaseType.None)
                return null;

            return Options.Prerelease ? BumpPrereleaseTag(releaseType) : BumpReleaseTag(releaseType);
        }

        private IGitTag BumpPrereleaseTag(ReleaseType releaseType) {
            IVersion newVersion = LatestTag.Bump(releaseType, Options).Version!;
            IVersion? prereleaseVersion = LatestPrereleaseTag.Version;

            return newVersion.IsReleaseEqual(prereleaseVersion)
                ? LatestPrereleaseTag.BumpPrerelease(Options)
                : LatestPrereleaseTag.Bump(releaseType, Options);
        }

        private IGitTag BumpReleaseTag(ReleaseType releaseType) {
            return LatestTag.Bump(releaseType, Options);
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
