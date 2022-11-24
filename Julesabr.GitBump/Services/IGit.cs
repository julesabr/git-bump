using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Julesabr.GitBump.Services {
    public interface IGit {
        IGitTag? LatestTag { get; }
        // IEnumerable<Commit> LatestCommits { get; }

        public static IGit Create(IRepository repository, string? tagPrefix = "v", string? tagSuffix = "") {
            IDictionary<ObjectId, IList<Tag>> tagsPerCommitId = TagsPerCommitId(repository);

            CommitFilter filter = new() {
                SortBy = CommitSortStrategies.Reverse
            };

            IGitTag? latestTag = (from commit in repository.Commits.QueryBy(filter)
                select AssignedTags(commit, tagsPerCommitId).FirstOrDefault(tag => tag.IsAnnotated)
                into annotatedTag
                where annotatedTag != null
                select IGitTag.Create(annotatedTag.FriendlyName, tagPrefix, tagSuffix)).FirstOrDefault();

            return new Git(latestTag, new List<Commit>());
        }

        private static IEnumerable<Tag> AssignedTags(GitObject commit, IDictionary<ObjectId, IList<Tag>> tags) {
            return !tags.ContainsKey(commit.Id) ? Enumerable.Empty<Tag>() : tags[commit.Id];
        }

        private static IDictionary<ObjectId, IList<Tag>> TagsPerCommitId(IRepository repo) {
            Dictionary<ObjectId, IList<Tag>> tagsPerCommitId = new();

            foreach (Tag tag in repo.Tags) {
                GitObject peeledTarget = tag.PeeledTarget;

                if (peeledTarget is not Commit)
                    continue;

                ObjectId commitId = peeledTarget.Id;

                if (!tagsPerCommitId.ContainsKey(commitId))
                    tagsPerCommitId.Add(commitId, new List<Tag>());

                tagsPerCommitId[commitId].Add(tag);
            }

            return tagsPerCommitId;
        }
    }
}
