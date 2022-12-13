using System.Collections.Generic;
using System.Linq;
using Julesabr.LibGit;

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
                .Select(tag => IGitTag.Create(tag.Name, options.Prefix, options.Suffix))
                .Where(tag => !tag.Version.IsPrerelease)
                .OrderByDescending(tag => tag)
                .FirstOrDefault();

            IGitTag? latestPrereleaseTag = null;
            if (options.Prerelease)
                latestPrereleaseTag = repository.Tags.Where(tag => tag.IsAnnotated)
                    .Select(tag => IGitTag.Create(tag.Name, options.Prefix, options.Suffix))
                    .Where(tag =>
                        tag.Version.IsPrerelease && tag.Version.PrereleaseBranch == repository.Head.Name)
                    .OrderByDescending(tag => tag)
                    .FirstOrDefault();

            IDictionary<string, IList<Tag>> tagsPerCommitId = TagsPerCommitId(repository);
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
            IDictionary<string, IList<Tag>> tagsPerCommitId
        ) {
            return repository.Commits.QueryBy(filter)
                .TakeWhile(commit => AssignedTags(commit, tagsPerCommitId)
                    .Where(t => t.IsAnnotated)
                    .All(t => t.Name != tag?.ToString()))
                .ToList();
        }

        private static IEnumerable<Tag> AssignedTags(Commit commit, IDictionary<string, IList<Tag>> tags) {
            return !tags.ContainsKey(commit.Sha) ? Enumerable.Empty<Tag>() : tags[commit.Sha];
        }

        private static IDictionary<string, IList<Tag>> TagsPerCommitId(IRepository repo) {
            Dictionary<string, IList<Tag>> tagsPerCommitId = new();

            foreach (Tag tag in repo.Tags) {
                Commit target = tag.Target;

                if (!tagsPerCommitId.ContainsKey(target.Sha))
                    tagsPerCommitId.Add(target.Sha, new List<Tag>());

                tagsPerCommitId[target.Sha].Add(tag);
            }

            return tagsPerCommitId;
        }
    }
}
