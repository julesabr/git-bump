using System;
using System.Collections;
using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump.Tests {
    public class QueryableCommitLogStub : IQueryableCommitLog {
        private readonly ICommitLog log;

        public QueryableCommitLogStub(IList<Commit> commits) {
            log = new CommitLogStub(commits);
        }

        public IEnumerator<Commit> GetEnumerator() {
            return log.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public CommitSortStrategies SortedBy { get; }

        public ICommitLog QueryBy(CommitFilter filter) {
            return log;
        }

        public IEnumerable<LogEntry> QueryBy(string path) {
            throw new NotImplementedException();
        }

        public IEnumerable<LogEntry> QueryBy(string path, CommitFilter filter) {
            throw new NotImplementedException();
        }
    }
}
