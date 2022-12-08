using System.Collections.Generic;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    internal sealed class GitDetails : IGitDetails {
        private const string CommitTypeRegexInclusive = @"^[a-zA-Z]+(?:\(.+\))?: ";
        private const string CommitTypeRegexExclusive = @"^[a-zA-Z]+";

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

        public IGitTag? LatestTag { get; }
        public IGitTag? LatestPrereleaseTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }

        private Options Options { get; }

        public IGitTag BumpTag() {
            if (LatestTag == null)
                return IGitTag.Create(IVersion.First, Options.Prefix, Options.Suffix);

            BumpType currentType = BumpType.None;
            foreach (Commit commit in LatestCommits) {
                string match = Regex.Match(commit.MessageShort, CommitTypeRegexInclusive).Value;
                match = Regex.Match(match, CommitTypeRegexExclusive).Value;

                if (!ConventionalCommits.Map.TryGetValue(match, out BumpType type))
                    type = BumpType.None;

                if (type > currentType)
                    currentType = type;
            }

            switch (currentType) {
                case BumpType.Patch:
                    return IGitTag.Create(LatestTag.Version.BumpPatch(), LatestTag.Prefix, LatestTag.Suffix);
                case BumpType.Minor:
                    return IGitTag.Create(LatestTag.Version.BumpMinor(), LatestTag.Prefix, LatestTag.Suffix);
            }

            return LatestTag;
        }
    }
}
