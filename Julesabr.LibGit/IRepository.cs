using System.Collections.Generic;
using System.Linq;

namespace Julesabr.LibGit {
    public interface IRepository {
        Branch Head { get; }
        IQueryableCommitLog Commits { get; }
        TagCollection Tags { get; }

        void ApplyTag(string tagName, string message);

        public static IRepository AtCurrentDirectory() {
            Branch head = new(Shell.Run("bash", "git branch --show-current"));
            IQueryableCommitLog commits = new QueryableCommitLog();

            IList<Tag> tagList = Shell.Run("bash", "git tag")
                .Split('\n')
                .Select(name =>
                    new Tag(
                        name,
                        Shell.Run("bash", $"git cat-file -t {name}") == "tag",
                        commits.First(c => Shell.Run("bash", $"git rev-list -n 1 {name}") == c.Sha)
                    )
                )
                .ToList();
            TagCollection tags = new(tagList);

            return new Repository(head, commits, tags);
        }
    }
}
