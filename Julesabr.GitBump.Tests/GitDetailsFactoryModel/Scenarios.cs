using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;

namespace Julesabr.GitBump.Tests.GitDetailsFactoryModel {
    internal static class Scenarios {
        [Given]
        public static GivenBuilder With(this Given<IGitDetails.Factory> @this) {
            return @this.With<GivenBuilder>();
        }

        [Given]
        public static GivenBuilder AGitTagFactoryForOptions(this GivenBuilder @this, Options options) {
            IGitTag.Factory gitTagFactory = Substitute.For<IGitTag.Factory>();

            Given.ReleaseTag1.ToString().Returns("v1.0.0");
            Given.ReleaseTag2.ToString().Returns("v1.1.0");
            Given.PrereleaseDefaultChannelTag1.ToString().Returns("v1.1.1.dev.1");
            Given.PrereleaseDefaultChannelTag2.ToString().Returns("v1.1.1.dev.2");
            Given.PrereleaseNonDefaultChannelTag.ToString().Returns("v1.1.1.staging.1");
            
            gitTagFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns((IGitTag)null!);
            gitTagFactory.Create("v1.0.0", options).Returns(Given.ReleaseTag1);
            gitTagFactory.Create("v1.1.0", options).Returns(Given.ReleaseTag2);
            gitTagFactory.Create("v1.1.1.dev.1", options).Returns(Given.PrereleaseDefaultChannelTag1);
            gitTagFactory.Create("v1.1.1.dev.2", options).Returns(Given.PrereleaseDefaultChannelTag2);
            gitTagFactory.Create("v1.1.1.staging.1", options).Returns(Given.PrereleaseNonDefaultChannelTag);

            gitTagFactory.CreateEmpty(options).Returns(Given.EmptyTag);

            @this.GitTagFactory = gitTagFactory;
            return @this;
        }

        [Given]
        public static GivenBuilder ARepositoryThatContainsAnnotatedTags(this GivenBuilder @this) {
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

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            @this.Repository = repository;
            return @this;
        }

        [Given]
        public static GivenBuilder ARepositoryThatContainsNoAnnotatedTags(this GivenBuilder @this) {
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

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            @this.Repository = repository;
            return @this;
        }

        [When]
        public static When<IGitDetails.Factory, IGitDetails> Create(this When<IGitDetails.Factory> @this, Options options) {
            return @this.AddResult(@this.SystemUnderTest.Create(options));
        }

        [Then]
        public static IGitDetails And(this IGitDetails @this) {
            return @this;
        }

        [Then]
        public static IGitDetails ShouldHaveValidLatestTag(this IGitDetails @this) {
            @this.LatestTag.Should().NotBeNull();
            @this.LatestTag.Should().Be(Given.ReleaseTag2);
            return @this;
        }

        [Then]
        public static IGitDetails ShouldHaveEmptyLatestTag(this IGitDetails @this) {
            @this.LatestTag.Should().NotBeNull();
            @this.LatestTag.Should().Be(Given.EmptyTag);
            return @this;
        }

        [Then]
        public static IGitDetails ShouldHaveValidLatestPrereleaseTag(this IGitDetails @this) {
            @this.LatestPrereleaseTag.Should().NotBeNull();
            @this.LatestPrereleaseTag.Should().Be(Given.PrereleaseDefaultChannelTag2);
            return @this;
        }

        [Then]
        public static IGitDetails ShouldHaveEmptyLatestPrereleaseTag(this IGitDetails @this) {
            @this.LatestPrereleaseTag.Should().NotBeNull();
            @this.LatestPrereleaseTag.Should().Be(Given.EmptyTag);
            return @this;
        }

        [Then]
        public static void ShouldHaveCommitsUpToLatestTagAsLatestCommits(this IGitDetails @this) {
            IList<Commit> latestCommits = @this.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(3);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        [Then]
        public static void ShouldHaveAllCommitsAsLatestCommits(this IGitDetails @this) {
            IList<Commit> latestCommits = @this.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(6);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 1");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 2");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 3");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 4");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 5");
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }
    }
}
