using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    public interface IGitDetails {
        IGitTag? LatestTag { get; }
        IEnumerable<Commit> LatestCommits { get; }

        public static IGitDetails Create(IRepository repository, string? tagPrefix = "v", string? tagSuffix = "") {
            IGitTag? latestTag = null;
            IList<Commit> latestCommits = new List<Commit>();

            IDictionary<ObjectId, IList<Tag>> tagsPerCommitId = TagsPerCommitId(repository);

            CommitFilter filter = new() {
                SortBy = CommitSortStrategies.Reverse
            };

            foreach (Commit commit in repository.Commits.QueryBy(filter)) {
                Tag? annotatedTag = AssignedTags(commit, tagsPerCommitId).FirstOrDefault(tag => tag.IsAnnotated);
                if (annotatedTag != null) {
                    latestTag = IGitTag.Create(annotatedTag.FriendlyName, tagPrefix, tagSuffix);
                    break;
                }

                latestCommits.Add(commit);
            }

            return new GitDetails(latestTag, latestCommits);
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
