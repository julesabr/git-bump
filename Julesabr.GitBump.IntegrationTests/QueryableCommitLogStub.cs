using System.Collections;
using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.IntegrationTests {
    public class QueryableCommitLogStub : IQueryableCommitLog {
        private readonly ICommitLog log;

        public QueryableCommitLogStub(IList<Commit> commits) {
            log = new CommitLogStub(commits);
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedMember.Global
        public CommitSortStrategies SortedBy { get; }

        public IEnumerator<Commit> GetEnumerator() {
            return log.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
