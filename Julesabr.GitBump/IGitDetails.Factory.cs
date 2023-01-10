using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public partial interface IGitDetails {
        public class Factory {
            private readonly IGitTag.Factory gitTagFactory;
            private readonly IRepository repository;

            public Factory(IGitTag.Factory gitTagFactory, IRepository repository) {
                this.gitTagFactory = gitTagFactory;
                this.repository = repository;
            }
            
            [Pure]
            public IGitDetails Create(Options options) {
                IGitTag emptyTag = gitTagFactory.CreateEmpty(options);
                IList<IGitTag> gitTags = repository.Tags
                    .Where(tag => tag.IsAnnotated)
                    .Select(tag => gitTagFactory.Create(tag.Name, options))
                    .ToList();

                IGitTag latestTag = gitTags
                    .Where(tag => !tag.Version!.IsPrerelease)
                    .OrderByDescending(tag => tag)
                    .FirstOrDefault() ?? emptyTag;
                
                IGitTag latestPrereleaseTag = emptyTag;
                if (options.Prerelease)
                    latestPrereleaseTag = gitTags
                        .Where(tag =>
                            tag.Version!.IsPrerelease && tag.Version!.PrereleaseChannel == options.Channel)
                        .OrderByDescending(tag => tag)
                        .FirstOrDefault() ?? emptyTag;
                
                IEnumerable<Commit> latestCommits = LatestCommitsSince(options.Prerelease ? latestPrereleaseTag : latestTag,
                    TagsPerCommitId());
                
                return new GitDetails(latestTag, latestPrereleaseTag, latestCommits, options);
            }

            private IEnumerable<Commit> LatestCommitsSince(
                IGitTag? tag,
                IDictionary<string, IList<Tag>> tagsPerCommitId
            ) {
                return repository.Commits
                    .TakeWhile(commit => AssignedTags(commit, tagsPerCommitId)
                        .Where(t => t.IsAnnotated)
                        .All(t => t.Name != tag?.ToString()))
                    .ToList();
            }

            private IEnumerable<Tag> AssignedTags(Commit commit, IDictionary<string, IList<Tag>> tags) {
                return !tags.ContainsKey(commit.Sha) ? Enumerable.Empty<Tag>() : tags[commit.Sha];
            }

            private IDictionary<string, IList<Tag>> TagsPerCommitId() {
                Dictionary<string, IList<Tag>> tagsPerCommitId = new();

                foreach (Tag tag in repository.Tags) {
                    Commit target = tag.Target;

                    if (!tagsPerCommitId.ContainsKey(target.Sha))
                        tagsPerCommitId.Add(target.Sha, new List<Tag>());

                    tagsPerCommitId[target.Sha].Add(tag);
                }

                return tagsPerCommitId;
            }
        }
    }
}
