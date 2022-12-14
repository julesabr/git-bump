using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Julesabr.LibGit {
    internal class QueryableCommitLog : IQueryableCommitLog {
        private readonly IEnumerable<Commit>? commits;

        public QueryableCommitLog(
            IEnumerable<Commit>? commits = null,
            CommitSortStrategies sortedBy = CommitSortStrategies.None
        ) {
            this.commits = commits;
            SortedBy = sortedBy;
        }

        private CommitSortStrategies SortedBy { get; }

        public IEnumerator<Commit> GetEnumerator() {
            CommitFilter filter = new() {
                SortBy = SortedBy
            };

            return commits == null ? QueryBy(filter).GetEnumerator() : commits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private ICommitLog QueryBy(CommitFilter filter) {
            string command = filter.SortBy switch {
                CommitSortStrategies.Topological => Shell.GitLogWithShaAndBodyInTopoOrder,
                CommitSortStrategies.Time => Shell.GitLogWithShaAndBodyInDateOrder,
                CommitSortStrategies.Reverse => Shell.GitLogWithShaAndBodyInReverse,
                _ => Shell.GitLogWithShaAndBody
            };

            return new QueryableCommitLog(
                GetFromShellCommand(command)
            );
        }

        private IEnumerable<Commit> GetFromShellCommand(string command) {
            return Shell.Run(command)
                .Split("<EOC>")
                .Select(s =>
                    new Commit(
                        SubstringUpToFirstSpace(s),
                        SubstringBetweenFirstSpaceAndFirstNewline(s),
                        SubstringFromFirstSpaceToEnd(s)
                    )
                );
        }

        private string SubstringUpToFirstSpace(string str) {
            return str[..str.IndexOf(' ')];
        }

        private string SubstringBetweenFirstSpaceAndFirstNewline(string str) {
            return str.Substring(str.IndexOf(' ') + 1, str.IndexOf('\n'));
        }

        private string SubstringFromFirstSpaceToEnd(string str) {
            return str[(str.IndexOf(' ') + 1)..];
        }
    }
}
