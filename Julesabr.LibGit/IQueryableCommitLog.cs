namespace Julesabr.LibGit {
    public interface IQueryableCommitLog : ICommitLog {
        ICommitLog QueryBy(CommitFilter filter);
    }
}
