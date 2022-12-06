using System.Collections.Generic;

namespace Julesabr.GitBump {
    internal static class ConventionalCommits {
        public static readonly IDictionary<string, BumpType> Map = new Dictionary<string, BumpType> {
            { "build", BumpType.Patch },
            { "ci", BumpType.Patch },
            { "style", BumpType.None },
            { "test", BumpType.None },
            { "chore", BumpType.None }
        };
    }
}
