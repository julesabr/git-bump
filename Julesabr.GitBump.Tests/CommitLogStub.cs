using System.Collections;
using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump.Tests {
    public class CommitLogStub : ICommitLog {
        private readonly IList<Commit> commits;

        public CommitLogStub(IList<Commit> commits) {
            this.commits = commits;
        }

        public IEnumerator<Commit> GetEnumerator() {
            return commits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public CommitSortStrategies SortedBy { get; }
    }
}
