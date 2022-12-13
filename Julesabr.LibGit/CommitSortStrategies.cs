using System;

namespace Julesabr.LibGit {
    [Flags]
    public enum CommitSortStrategies {
        None,
        Topological,
        Time,
        Reverse
    }
}
