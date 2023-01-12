using System.Collections;
using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests {
    public class CommitLogStub : ICommitLog {
        private readonly IList<Commit> commits;

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedMember.Global
        public CommitSortStrategies SortedBy { get; }

        public CommitLogStub(IList<Commit> commits) {
            this.commits = commits;
        }

        public IEnumerator<Commit> GetEnumerator() {
            return commits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
