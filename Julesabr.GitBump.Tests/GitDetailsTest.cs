using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LibGit2Sharp;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitDetailsTest {
        [Test]
        public void Create_GivenARepositoryWith1TagPerCommit_ThenReturnGitWithLatestAnnotatedTag() {
            IRepository repository = RepositoryWith2AnnotatedTags1LightweightTagAnd1TagPerCommit();
            IGitDetails gitDetails = IGitDetails.Create(repository);
            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1, 1)));
        }

        [Test]
        public void Create_GivenARepositoryWith2TagsPerCommit_ThenReturnGitWithLatestAnnotatedTag() {
            IRepository repository = RepositoryWith1AnnotatedTag1LightweightTagAnd2TagsPerCommit();
            IGitDetails gitDetails = IGitDetails.Create(repository);
            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1)));
        }

        [Test]
        public void Create_GivenARepositoryWithNoAnnotatedTags_ThenReturnGitWithLatestTagAsNull() {
            IRepository repository = RepositoryWith0AnnotatedTagsAnd1LightweightTag();
            IGitDetails gitDetails = IGitDetails.Create(repository);
            gitDetails.LatestTag.Should().BeNull();
        }

        [Test]
        public void Create_GivenAnyRepository_ThenReturnGitWithLatestCommits() {
            IRepository repository = RepositoryWith2AnnotatedTags1LightweightTagAnd1TagPerCommit();

            IGitDetails gitDetails = IGitDetails.Create(repository);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(3);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        private IRepository RepositoryWith2AnnotatedTags1LightweightTagAnd1TagPerCommit() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Id.Returns(new ObjectId("f7570139e573b36646a8f3058fda1f3da6a99b82"));
            commit1.Message.Returns("Commit 1");

            Tag tag1 = Substitute.For<Tag>();
            tag1.FriendlyName.Returns("v1.0.0");
            tag1.IsAnnotated.Returns(true);
            tag1.PeeledTarget.Returns(commit1);

            Commit commit2 = Substitute.For<Commit>();
            commit2.Id.Returns(new ObjectId("a52eed7e50c806a5ab4e4397212acbc37ab926f8"));
            commit2.Message.Returns("Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Id.Returns(new ObjectId("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d"));
            commit3.Message.Returns("Commit 3");

            Tag tag2 = Substitute.For<Tag>();
            tag2.FriendlyName.Returns("v1.1.0");
            tag2.IsAnnotated.Returns(true);
            tag2.PeeledTarget.Returns(commit3);

            Commit commit4 = Substitute.For<Commit>();
            commit4.Id.Returns(new ObjectId("f3114cd9cf56d31996c682ed1912c8cffe9fa842"));
            commit4.Message.Returns("Commit 4");

            Tag tag3 = Substitute.For<Tag>();
            tag3.FriendlyName.Returns("foo");
            tag3.IsAnnotated.Returns(false);
            tag3.PeeledTarget.Returns(commit4);

            Commit commit5 = Substitute.For<Commit>();
            commit5.Id.Returns(new ObjectId("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7"));
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Id.Returns(new ObjectId("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b"));
            commit6.Message.Returns("Commit 6");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag3);
            tags.Add(tag2);
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            return repository;
        }

        private IRepository RepositoryWith1AnnotatedTag1LightweightTagAnd2TagsPerCommit() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Id.Returns(new ObjectId("f7570139e573b36646a8f3058fda1f3da6a99b82"));
            commit1.Message.Returns("Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Id.Returns(new ObjectId("a52eed7e50c806a5ab4e4397212acbc37ab926f8"));
            commit2.Message.Returns("Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Id.Returns(new ObjectId("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d"));
            commit3.Message.Returns("Commit 3");

            Tag tag1 = Substitute.For<Tag>();
            tag1.FriendlyName.Returns("v1.0.0");
            tag1.IsAnnotated.Returns(true);
            tag1.PeeledTarget.Returns(commit3);

            Tag tag2 = Substitute.For<Tag>();
            tag2.FriendlyName.Returns("foo");
            tag2.IsAnnotated.Returns(false);
            tag2.PeeledTarget.Returns(commit3);

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag2);
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            return repository;
        }

        private IRepository RepositoryWith0AnnotatedTagsAnd1LightweightTag() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Id.Returns(new ObjectId("f7570139e573b36646a8f3058fda1f3da6a99b82"));
            commit1.Message.Returns("Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Id.Returns(new ObjectId("a52eed7e50c806a5ab4e4397212acbc37ab926f8"));
            commit2.Message.Returns("Commit 2");

            Tag tag1 = Substitute.For<Tag>();
            tag1.FriendlyName.Returns("foo");
            tag1.IsAnnotated.Returns(false);
            tag1.PeeledTarget.Returns(commit2);

            Commit commit3 = Substitute.For<Commit>();
            commit3.Id.Returns(new ObjectId("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d"));
            commit3.Message.Returns("Commit 3");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            return repository;
        }
    }
}
