using System.Collections.Generic;
using System.Linq;

namespace Julesabr.LibGit {
    internal class Repository : IRepository {
        private Branch? head;
        private TagCollection? tags;

        public Branch Head => head ??= new Branch(Shell.Run(Shell.GitShowCurrentBranch));
        public TagCollection Tags => GetTagCollection();

        public IQueryableCommitLog Commits { get; } = new QueryableCommitLog();
        public INetwork Network { get; } = new Network();

        public void ApplyTag(string tagName, string message) {
            Shell.Run(string.Format(Shell.GitApplyAnnotatedTag, tagName, message));
        }

        private TagCollection GetTagCollection() {
            // ReSharper disable once InvertIf
            if (tags == null) {
                IList<Tag> tagList = Shell.Run(Shell.GitTagList)
                    .Split('\n')
                    .Select(name =>
                        new Tag(
                            name,
                            Shell.Run(string.Format(Shell.GitCatObjectType, name)) == "tag",
                            Commits.First(c => Shell.Run(string.Format(Shell.GitGetCommitSha, name)) == c.Sha)
                        )
                    )
                    .ToList();
                tags = new TagCollection(tagList);
            }

            return tags;
        }
    }
}
