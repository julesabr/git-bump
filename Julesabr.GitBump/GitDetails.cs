using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    internal sealed class GitDetails : IGitDetails {
        public GitDetails(IGitTag? latestTag, IGitTag? latestPrereleaseTag, IEnumerable<Commit> latestCommits) {
            LatestTag = latestTag;
            LatestPrereleaseTag = latestPrereleaseTag;
            LatestCommits = latestCommits;
        }

        public IGitTag? LatestTag { get; }
        public IGitTag? LatestPrereleaseTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }
    }
}
