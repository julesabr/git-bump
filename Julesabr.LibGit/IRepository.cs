using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Julesabr.LibGit {
    public interface IRepository {
        Branch Head { get; }
        IQueryableCommitLog Commits { get; }
        TagCollection Tags { get; }
        INetwork Network { get; }

        void ApplyTag(string tagName, string message);

        [Pure]
        public static IRepository AtCurrentDirectory() {
            Branch head = new(Shell.Run(Shell.GitShowCurrentBranch));
            IQueryableCommitLog commits = new QueryableCommitLog();

            IList<Tag> tagList = Shell.Run(Shell.GitTagList)
                .Split('\n')
                .Select(name =>
                    new Tag(
                        name,
                        Shell.Run(string.Format(Shell.GitCatObjectType, name)) == "tag",
                        commits.First(c => Shell.Run(string.Format(Shell.GitGetCommitSha, name)) == c.Sha)
                    )
                )
                .ToList();
            TagCollection tags = new(tagList);

            INetwork network = INetwork.Create();

            return new Repository(head, commits, tags, network);
        }
    }
}
