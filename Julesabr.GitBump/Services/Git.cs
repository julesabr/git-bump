using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump.Services {
    internal sealed class Git : IGit {
        public Git(IGitTag? latestTag, IEnumerable<Commit> latestCommits) {
            LatestTag = latestTag;
            LatestCommits = latestCommits;
        }

        public IGitTag? LatestTag { get; }
        public IEnumerable<Commit> LatestCommits { get; }
    }
}
