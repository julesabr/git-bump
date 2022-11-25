using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    internal sealed class GitDetails : IGitDetails {
        public GitDetails(IGitTag? latestTag, IEnumerable<Commit> latestCommits) {
            LatestTag = latestTag;
            LatestCommits = latestCommits;
        }

        public IGitTag? LatestTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }
    }
}
