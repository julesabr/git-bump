namespace Julesabr.LibGit {
    internal class Repository : IRepository {
        public Repository(Branch head, IQueryableCommitLog commits, TagCollection tags) {
            Head = head;
            Commits = commits;
            Tags = tags;
        }

        public Branch Head { get; }
        public IQueryableCommitLog Commits { get; }
        public TagCollection Tags { get; }

        public void ApplyTag(string tagName, string message) {
            Shell.Run(Shell.GitApplyAnnotatedTag);
        }
    }
}
