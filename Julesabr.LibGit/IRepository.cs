using JetBrains.Annotations;

namespace Julesabr.LibGit {
    public interface IRepository {
        Branch Head { get; }
        IQueryableCommitLog Commits { get; }
        TagCollection Tags { get; }
        INetwork Network { get; }

        void ApplyTag(string tagName, string message);

        [Pure]
        public static IRepository Create() {
            return new Repository();
        }
    }
}
