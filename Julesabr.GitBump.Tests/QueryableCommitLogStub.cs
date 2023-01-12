using System.Collections;
using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests {
    public class QueryableCommitLogStub : IQueryableCommitLog {
        private readonly ICommitLog log;

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedMember.Global
        public CommitSortStrategies SortedBy { get; }

        public QueryableCommitLogStub(IList<Commit> commits) {
            log = new CommitLogStub(commits);
        }

        public IEnumerator<Commit> GetEnumerator() {
            return log.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
