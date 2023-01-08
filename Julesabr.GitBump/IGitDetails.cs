using System.Collections.Generic;
using JetBrains.Annotations;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public partial interface IGitDetails {
        IGitTag? LatestTag { get; }
        IGitTag? LatestPrereleaseTag { get; }
        IEnumerable<Commit> LatestCommits { get; }

        [Pure]
        IGitTag? BumpTag();
    }
}
