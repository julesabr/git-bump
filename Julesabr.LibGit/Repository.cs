namespace Julesabr.LibGit {
    internal class Repository : IRepository {
        public Repository(Branch head, IQueryableCommitLog commits, TagCollection tags, INetwork network) {
            Head = head;
            Commits = commits;
            Tags = tags;
            Network = network;
        }

        public Branch Head { get; }
        public IQueryableCommitLog Commits { get; }
        public TagCollection Tags { get; }
        public INetwork Network { get; }

        public void ApplyTag(string tagName, string message) {
            Shell.Run(string.Format(Shell.GitApplyAnnotatedTag, tagName, message));
        }
    }
}
