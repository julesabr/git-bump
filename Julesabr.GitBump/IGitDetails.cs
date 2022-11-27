using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    public interface IGitDetails {
        IGitTag? LatestTag { get; }
        IGitTag? LatestPrereleaseTag { get; }
        IEnumerable<Commit> LatestCommits { get; }

        IGitTag BumpTag();

        public static IGitDetails Create(
            IGitTag? latestTag,
            IGitTag? latestPrereleaseTag,
            IEnumerable<Commit> latestCommits,
            Options options
        ) {
            return new GitDetails(latestTag, latestPrereleaseTag, latestCommits, options);
        }

        public static IGitDetails Create(IRepository repository, Options options) {
            IGitTag? latestTag = repository.Tags.Where(tag => tag.IsAnnotated)
                .Select(tag => IGitTag.Create(tag.FriendlyName, options.Prefix, options.Suffix))
                .Where(tag => !tag.Version.IsPrerelease)
                .OrderByDescending(tag => tag)
                .FirstOrDefault();

            IGitTag? latestPrereleaseTag = null;
            if (options.Prerelease)
                latestPrereleaseTag = repository.Tags.Where(tag => tag.IsAnnotated)
                    .Select(tag => IGitTag.Create(tag.FriendlyName, options.Prefix, options.Suffix))
                    .Where(tag =>
                        tag.Version.IsPrerelease && tag.Version.PrereleaseBranch == repository.Head.FriendlyName)
                    .OrderByDescending(tag => tag)
                    .FirstOrDefault();
            
            IDictionary<ObjectId, IList<Tag>> tagsPerCommitId = TagsPerCommitId(repository);
            CommitFilter filter = new() {
                SortBy = CommitSortStrategies.Reverse
            };

            IEnumerable<Commit> latestCommits = LatestCommitsSince(options.Prerelease ? latestPrereleaseTag : latestTag,
                repository, filter, tagsPerCommitId);

            return new GitDetails(latestTag, latestPrereleaseTag, latestCommits, options);
        }

        private static IEnumerable<Commit> LatestCommitsSince(
            IGitTag? tag,
            IRepository repository,
            CommitFilter filter,
            IDictionary<ObjectId, IList<Tag>> tagsPerCommitId
        ) {
            return repository.Commits.QueryBy(filter)
                .TakeWhile(commit => AssignedTags(commit, tagsPerCommitId)
                    .Where(t => t.IsAnnotated)
                    .All(t => t.FriendlyName != tag?.ToString()))
                .ToList();
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
