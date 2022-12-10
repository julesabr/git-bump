using System.Collections.Generic;

namespace Julesabr.GitBump {
    internal static class ConventionalCommits {
        public static readonly IDictionary<string, ReleaseType> Map = new Dictionary<string, ReleaseType> {
            { "feat", ReleaseType.Minor },
            { "fix", ReleaseType.Patch },
            { "revert", ReleaseType.Patch },
            { "docs", ReleaseType.Patch },
            { "refactor", ReleaseType.Patch },
            { "perf", ReleaseType.Patch },
            { "build", ReleaseType.Patch },
            { "ci", ReleaseType.Patch },
            { "style", ReleaseType.None },
            { "test", ReleaseType.None },
            { "chore", ReleaseType.None }
        };
    }
}
