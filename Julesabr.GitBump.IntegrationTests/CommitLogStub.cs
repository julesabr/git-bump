using System.Collections;
using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.IntegrationTests {
    public class CommitLogStub : ICommitLog {
        private readonly IList<Commit> commits;

        public CommitLogStub(IList<Commit> commits) {
            this.commits = commits;
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedMember.Global
        public CommitSortStrategies SortedBy { get; }

        public IEnumerator<Commit> GetEnumerator() {
            return commits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
