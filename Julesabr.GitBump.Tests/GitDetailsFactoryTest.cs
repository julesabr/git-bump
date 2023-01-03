using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitDetailsFactoryTest {
        private IGitTag.Factory gitTagFactory = null!;
        private IRepository repository = null!;
        private IGitDetails.Factory gitDetailsFactory = null!;

        [SetUp]
        public void Setup() {
            gitTagFactory = Substitute.For<IGitTag.Factory>();
            gitTagFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns((IGitTag)null!);
            gitTagFactory.Create("v1.0.0", "v", "").Returns(new GitTag(new Version(1, 0, 0), "v", ""));
            gitTagFactory.Create("v1.1.0", "v", "").Returns(new GitTag(new Version(1, 1, 0), "v", ""));
            gitTagFactory.Create("v1.1.1.dev.1", "v", "").Returns(new GitTag(new Version(1, 1, 1, "dev", 1), "v", ""));
            gitTagFactory.Create("v1.1.1.dev.2", "v", "").Returns(new GitTag(new Version(1, 1, 1, "dev", 2), "v", ""));
            gitTagFactory.Create("v1.1.1.staging.1", "v", "").Returns(new GitTag(new Version(1, 1, 1, "staging", 1), "v", ""));
        }
        
        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSeparateCommitsAndThisIsNotAPrerelease_ThenReturnGitDetailsWithLatestTag() {
            repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);

            gitDetails.LatestTag.Should().Be(new GitTag(new Version(1, 1, 0), "v", ""));
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSeparateCommitsAndThisIsAPrerelease_ThenReturnGitDetailsWithLatestTagAndLatestPrereleaseTag() {
            repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = true,
                Channel = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);

            gitDetails.LatestTag.Should().Be(new GitTag(new Version(1, 1, 0), "v", ""));
            gitDetails.LatestPrereleaseTag.Should()
                .Be(new GitTag(new Version(1, 1, 1, "dev", 2), "v", ""));
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSameCommitAndThisIsNotAPrerelease_ThenReturnGitDetailsWithLatestTag() {
            repository = RepositoryWithAnnotatedTagsOnSameCommit();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);

            gitDetails.LatestTag.Should().Be(new GitTag(new Version(1, 1, 0), "v", ""));
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSameCommitAndThisIsAPrerelease_ThenReturnGitDetailsWithLatestTagAndLatestPrereleaseTag() {
            repository = RepositoryWithAnnotatedTagsOnSameCommit();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = true,
                Channel = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);

            gitDetails.LatestTag.Should().Be(new GitTag(new Version(1, 1, 0), "v", ""));
            gitDetails.LatestPrereleaseTag.Should()
                .Be(new GitTag(new Version(1, 1, 1, "dev", 2), "v", ""));
        }

        [Test]
        public void Create_GivenARepositoryWithNoAnnotatedTags_ThenReturnGitDetailsWithLatestTagAsNull() {
            repository = RepositoryWithNoAnnotatedTags();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);

            gitDetails.LatestTag.Should().BeNull();
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsAndIsNotAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestTag() {
            repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(3);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        [Test]
        public void
            Create_GivenARepositoryWithNoAnnotatedTagsAndIsNotAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestTag() {
            repository = RepositoryWithNoAnnotatedTags();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(6);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 1");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 2");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 3");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsAndIsAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestPrereleaseTag() {
            repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = true,
                Channel = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(1);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        [Test]
        public void
            Create_GivenARepositoryWithNoAnnotatedTagsAndIsAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestTag() {
            repository = RepositoryWithNoAnnotatedTags();
            gitDetailsFactory = new IGitDetails.Factory(gitTagFactory, repository);
            Options options = new() {
                Prerelease = true,
                Channel = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = gitDetailsFactory.Create(options);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(6);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 1");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 2");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 3");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        private IRepository RepositoryWithAnnotatedTagsOnSeparateCommits() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("f7570139e573b36646a8f3058fda1f3da6a99b82");
            commit1.Message.Returns("Commit 1");

            Tag tag1 = Substitute.For<Tag>();
            tag1.Name.Returns("v1.0.0");
            tag1.IsAnnotated.Returns(true);
            tag1.Target.Returns(commit1);

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("a52eed7e50c806a5ab4e4397212acbc37ab926f8");
            commit2.Message.Returns("Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d");
            commit3.Message.Returns("Commit 3");

            Tag tag2 = Substitute.For<Tag>();
            tag2.Name.Returns("v1.1.0");
            tag2.IsAnnotated.Returns(true);
            tag2.Target.Returns(commit3);

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("f3114cd9cf56d31996c682ed1912c8cffe9fa842");
            commit4.Message.Returns("Commit 4");

            Tag tag3 = Substitute.For<Tag>();
            tag3.Name.Returns("foo");
            tag3.IsAnnotated.Returns(false);
            tag3.Target.Returns(commit4);

            Tag tag4 = Substitute.For<Tag>();
            tag4.Name.Returns("v1.1.1.dev.1");
            tag4.IsAnnotated.Returns(true);
            tag4.Target.Returns(commit4);

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Tag tag5 = Substitute.For<Tag>();
            tag5.Name.Returns("v1.1.1.dev.2");
            tag5.IsAnnotated.Returns(true);
            tag5.Target.Returns(commit5);

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            Tag tag6 = Substitute.For<Tag>();
            tag6.Name.Returns("v1.1.1.staging.1");
            tag6.IsAnnotated.Returns(true);
            tag6.Target.Returns(commit6);

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag6);
            tags.Add(tag5);
            tags.Add(tag4);
            tags.Add(tag3);
            tags.Add(tag2);
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository localRepository = Substitute.For<IRepository>();
            localRepository.Commits.Returns(log);
            localRepository.Tags.Returns(tagCollection);

            return localRepository;
        }

        private IRepository RepositoryWithAnnotatedTagsOnSameCommit() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("f7570139e573b36646a8f3058fda1f3da6a99b82");
            commit1.Message.Returns("Commit 1");

            Tag tag1 = Substitute.For<Tag>();
            tag1.Name.Returns("v1.0.0");
            tag1.IsAnnotated.Returns(true);
            tag1.Target.Returns(commit1);

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("a52eed7e50c806a5ab4e4397212acbc37ab926f8");
            commit2.Message.Returns("Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d");
            commit3.Message.Returns("Commit 3");

            Tag tag2 = Substitute.For<Tag>();
            tag2.Name.Returns("v1.1.0");
            tag2.IsAnnotated.Returns(true);
            tag2.Target.Returns(commit3);

            Tag tag3 = Substitute.For<Tag>();
            tag3.Name.Returns("foo");
            tag3.IsAnnotated.Returns(false);
            tag3.Target.Returns(commit3);

            Tag tag4 = Substitute.For<Tag>();
            tag4.Name.Returns("v1.1.1.dev.1");
            tag4.IsAnnotated.Returns(true);
            tag4.Target.Returns(commit3);

            Tag tag5 = Substitute.For<Tag>();
            tag5.Name.Returns("v1.1.1.dev.2");
            tag5.IsAnnotated.Returns(true);
            tag5.Target.Returns(commit3);

            Tag tag6 = Substitute.For<Tag>();
            tag6.Name.Returns("v1.1.1.staging.1");
            tag6.IsAnnotated.Returns(true);
            tag6.Target.Returns(commit3);

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("f3114cd9cf56d31996c682ed1912c8cffe9fa842");
            commit4.Message.Returns("Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag6);
            tags.Add(tag5);
            tags.Add(tag4);
            tags.Add(tag3);
            tags.Add(tag2);
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository localRepository = Substitute.For<IRepository>();
            localRepository.Commits.Returns(log);
            localRepository.Tags.Returns(tagCollection);

            return localRepository;
        }

        private IRepository RepositoryWithNoAnnotatedTags() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("f7570139e573b36646a8f3058fda1f3da6a99b82");
            commit1.Message.Returns("Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("a52eed7e50c806a5ab4e4397212acbc37ab926f8");
            commit2.Message.Returns("Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("81d53a0f3294c1050ed6f4ee44fd2f0763e1d27d");
            commit3.Message.Returns("Commit 3");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("f3114cd9cf56d31996c682ed1912c8cffe9fa842");
            commit4.Message.Returns("Commit 4");

            Tag tag1 = Substitute.For<Tag>();
            tag1.Name.Returns("foo");
            tag1.IsAnnotated.Returns(false);
            tag1.Target.Returns(commit4);

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            IRepository localRepository = Substitute.For<IRepository>();
            localRepository.Commits.Returns(log);
            localRepository.Tags.Returns(tagCollection);

            return localRepository;
        }
    }
}
