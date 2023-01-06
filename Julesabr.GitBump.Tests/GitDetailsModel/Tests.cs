using System.Collections.Generic;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    public class Tests {
        private IGitTag.Factory gitTagFactory = null!;

        [SetUp]
        public void Setup() {
            gitTagFactory = Substitute.For<IGitTag.Factory>();
        }
        
        #region Release

        [Test]
        public void
            BumpTag_WithNoLatestTag_LatestCommitsThatContainAFeature_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForFirstVersion() {
            Given.GitDetails
                .With()
                .LatestCommitsThatContainAFeature()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.First())
                .Then()
                .TheResultShouldBeGitTagForVersion(IVersion.First);
        }

        [Test]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainNoSignificantChange_AndOptionsForRelease_ThenTheResult_Should_BeNull() {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainNoSignificantChange()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.First())
                .Then()
                .TheResult
                .Should()
                .BeNull();
        }

        [Test]
        [TestCase("ci")]
        [TestCase("build")]
        [TestCase("perf")]
        [TestCase("refactor")]
        [TestCase("docs")]
        [TestCase("revert")]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainALesserPatchChange_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForNextPatchVersion(string change) {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContain(change)
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.NextPatch())
                .Then()
                .TheResultShouldBeGitTagForVersion(Given.NextPatchVersion);
        }

        [Test]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainABugFix_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForNextPatchVersion() {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABugFix()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.NextPatch())
                .Then()
                .TheResultShouldBeGitTagForVersion(Given.NextPatchVersion);
        }
        
        [Test]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainAFeature_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForNextMinorVersion() {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainAFeature()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.NextMinor())
                .Then()
                .TheResultShouldBeGitTagForVersion(Given.NextMinorVersion);
        }
        
        [Test]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainABreakingChangeUsingExclamation_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForNextMajorVersion() {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABreakingChangeUsingExclamation()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.NextMajor())
                .Then()
                .TheResultShouldBeGitTagForVersion(Given.NextMajorVersion);
        }
        
        [Test]
        public void
            BumpTag_WithALatestTag_LatestCommitsThatContainABreakingChangeUsingMessageFooter_AndOptionsForRelease_ThenTheResult_Should_Be_GitTagForNextMajorVersion() {
            Given.GitDetails
                .With()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABreakingChangeUsingMessageFooter()
                .And()
                .OptionsForRelease()
                .When()
                .BumpTagForVersion(Given.AVersion.NextMajor())
                .Then()
                .TheResultShouldBeGitTagForVersion(Given.NextMajorVersion);
        }

        #endregion

        // #region Prerelease
        //
        // [Test]
        // public void BumpTag_WhenLatestTagIsNull_AndThisIsAPrerelease_ThenReturnFirstPrereleaseTag() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IEnumerable<Commit> latestCommits = CommitsWithFeature();
        //     IGitDetails details = new GitDetails(null, null, latestCommits,
        //         options);
        //
        //     details.BumpTag(gitTagFactory)
        //         .Should()
        //         .Be(new GitTag(
        //             IVersion.From(IVersion.First.Major, IVersion.First.Minor, IVersion.First.Patch, options.Channel, 1),
        //             options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_AndTheLatestCommitsContainNoSignificantChange_ThenReturnNull() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = Substitute.For<IGitTag>();
        //     IGitTag latestPrereleaseTag = Substitute.For<IGitTag>();
        //     IEnumerable<Commit> latestCommits = CommitsWithNoSignificantChange();
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag(gitTagFactory)
        //         .Should()
        //         .BeNull();
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainACiChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestPrereleaseTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = Substitute.For<IGitTag>();
        //     IGitTag latestPrereleaseTag = Substitute.For<IGitTag>();
        //     IVersion latestVersion = Substitute.For<IVersion>();
        //     IVersion latestPrereleaseVersion = Substitute.For<IVersion>();
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("e24d30b81e487aa28d8bfa574c15c2c074a507d4");
        //     commit.Message.Returns("ci: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     latestTag.Version.Returns(latestVersion);
        //     latestVersion.Bump(ReleaseType.Patch).Returns(IVersion.From(2, 1, 4));
        //     latestPrereleaseTag.Version.Returns(latestPrereleaseVersion);
        //     latestPrereleaseVersion.Major.Returns((ushort)2);
        //     latestPrereleaseVersion.Minor.Returns((ushort)1);
        //     latestPrereleaseVersion.Patch.Returns((ushort)4);
        //     latestPrereleaseVersion.BumpPrerelease().Returns(IVersion.From(2, 1, 4, options.Channel, 6));
        //     latestPrereleaseTag.Prefix.Returns(options.Prefix);
        //     latestPrereleaseTag.Suffix.Returns(options.Suffix);
        //     gitTagFactory.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix)
        //         .Returns(new GitTag(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        //
        //     details.BumpTag(gitTagFactory)
        //         .Should()
        //         .Be(new GitTag(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainACiChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestPrereleaseTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = Substitute.For<IGitTag>();
        //     IGitTag latestPrereleaseTag = Substitute.For<IGitTag>();
        //     IVersion latestVersion = Substitute.For<IVersion>();
        //     IVersion latestPrereleaseVersion = Substitute.For<IVersion>();
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("e24d30b81e487aa28d8bfa574c15c2c074a507d4");
        //     commit.Message.Returns("ci: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     latestTag.Version.Returns(latestVersion);
        //     latestVersion.Bump(ReleaseType.Patch).Returns(IVersion.From(2, 1, 4));
        //     latestPrereleaseTag.Version.Returns(latestPrereleaseVersion);
        //     latestPrereleaseVersion.Major.Returns((ushort)2);
        //     latestPrereleaseVersion.Minor.Returns((ushort)1);
        //     latestPrereleaseVersion.Patch.Returns((ushort)3);
        //     latestPrereleaseVersion.Bump(ReleaseType.Patch).Returns(IVersion.From(2, 1, 4, options.Channel, 1));
        //     latestPrereleaseTag.Prefix.Returns(options.Prefix);
        //     latestPrereleaseTag.Suffix.Returns(options.Suffix);
        //     gitTagFactory.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix)
        //         .Returns(new GitTag(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        //
        //     details.BumpTag(gitTagFactory)
        //         .Should()
        //         .Be(new GitTag(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABuildChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("build: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABuildChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("build: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAPerfChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("perf: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAPerfChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("perf: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARefactorChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("refactor: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARefactorChange_AndTheBumpedVersionDoesMotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("refactor: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainADocsChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("docs: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainADocsChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("docs: Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARevertChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("revert(calc): Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARevertChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IList<Commit> latestCommits = CommitsWithNoSignificantChange();
        //
        //     Commit commit = Substitute.For<Commit>();
        //     commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
        //     commit.Message.Returns("revert(calc): Commit 8");
        //     latestCommits.Add(commit);
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABugFix_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBugFix();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABugFix_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBugFix();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAFeature_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 2, 0, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithFeature();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 2, 0, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAFeature_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMinorBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithFeature();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(2, 2, 0, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingExclamation_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingExclamation();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingExclamation_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMajorBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingExclamation();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingMessageFooter_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingMessageFooter();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 6), options.Prefix, options.Suffix));
        // }
        //
        // [Test]
        // public void
        //     BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingMessageFooter_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMajorBump() {
        //     Options options = new() {
        //         Prerelease = true,
        //         Channel = "dev",
        //         Prefix = "v",
        //         Suffix = ""
        //     };
        //     IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
        //     IGitTag latestPrereleaseTag =
        //         IGitTag.Create(IVersion.From(2, 1, 3, options.Channel, 5), options.Prefix, options.Suffix);
        //     IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingMessageFooter();
        //
        //     IGitDetails details = new GitDetails(latestTag, latestPrereleaseTag, latestCommits,
        //         options);
        //
        //     details.BumpTag()
        //         .Should()
        //         .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Channel, 1), options.Prefix, options.Suffix));
        // }
        //
        // #endregion

        private IEnumerable<Commit> CommitsWithBreakingChangeUsingMessageFooter() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("85c7b488da8d14fe96bac1e399b56bf2a6dea990");
            commit1.Message.Returns("style: Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("86017adac2e16d9819e0be278474a09a90347b54");
            commit2.Message.Returns("fix(calc): Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("5e32d2a37d46e9341b4f9afb5d28f13074a06eb7");
            commit3.Message.Returns("feat: Commit 3");
            commit3.MessageFull.Returns(
                "feat: Commit 3\n\nCommit Body\n\nBREAKING CHANGE: this breaks some other change");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("09a6f6432e7ae9a1c793db85bba1ef47c1b20787");
            commit4.Message.Returns("test(calc): Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("1cabd5900830c8cd2f437d5183094f01ed38a0f0");
            commit7.Message.Returns("chore: Commit 7");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            return commits;
        }

        private IEnumerable<Commit> CommitsWithBreakingChangeUsingExclamation() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("85c7b488da8d14fe96bac1e399b56bf2a6dea990");
            commit1.Message.Returns("style: Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("86017adac2e16d9819e0be278474a09a90347b54");
            commit2.Message.Returns("fix(calc): Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("5e32d2a37d46e9341b4f9afb5d28f13074a06eb7");
            commit3.Message.Returns("feat: Commit 3");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("09a6f6432e7ae9a1c793db85bba1ef47c1b20787");
            commit4.Message.Returns("test(calc)!: Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("1cabd5900830c8cd2f437d5183094f01ed38a0f0");
            commit7.Message.Returns("chore: Commit 7");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            return commits;
        }

        private IEnumerable<Commit> CommitsWithFeature() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("85c7b488da8d14fe96bac1e399b56bf2a6dea990");
            commit1.Message.Returns("style: Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("86017adac2e16d9819e0be278474a09a90347b54");
            commit2.Message.Returns("fix(calc): Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("5e32d2a37d46e9341b4f9afb5d28f13074a06eb7");
            commit3.Message.Returns("feat: Commit 3");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("09a6f6432e7ae9a1c793db85bba1ef47c1b20787");
            commit4.Message.Returns("test(calc): Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("1cabd5900830c8cd2f437d5183094f01ed38a0f0");
            commit7.Message.Returns("chore: Commit 7");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            return commits;
        }

        private IEnumerable<Commit> CommitsWithBugFix() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("85c7b488da8d14fe96bac1e399b56bf2a6dea990");
            commit1.Message.Returns("style: Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("86017adac2e16d9819e0be278474a09a90347b54");
            commit2.Message.Returns("fix(calc): Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("421e029b331f50520417935ce70b8e0d7f405039");
            commit3.Message.Returns("test: Commit 3");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("09a6f6432e7ae9a1c793db85bba1ef47c1b20787");
            commit4.Message.Returns("test(calc): Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("b737f5c1096f56f0ecb3496204fc3182fdcc9cf7");
            commit5.Message.Returns("Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ddb048d1afed2c6beb3d8abc4c0a1f0d9a8de18b");
            commit6.Message.Returns("Commit 6");

            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("1cabd5900830c8cd2f437d5183094f01ed38a0f0");
            commit7.Message.Returns("chore: Commit 7");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            return commits;
        }

        private IList<Commit> CommitsWithNoSignificantChange() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("85c7b488da8d14fe96bac1e399b56bf2a6dea990");
            commit1.Message.Returns("style: Commit 1");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("c780f3549f895959f0afbb37d9cace81720cdb28");
            commit2.Message.Returns("style(calc): Commit 2");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("421e029b331f50520417935ce70b8e0d7f405039");
            commit3.Message.Returns("test: Commit 3");

            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("09a6f6432e7ae9a1c793db85bba1ef47c1b20787");
            commit4.Message.Returns("test(calc): Commit 4");

            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("8de7d116af7ed78a12cab3e0d61f5d69bb697a3a");
            commit5.Message.Returns("chore: Commit 5");

            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("ee993711b8cedc1876f8806302aa19c95e090e80");
            commit6.Message.Returns("chore(calc): Commit 6");

            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("662cb0ec5429f1d076fadaebc2f637c5ac4a8c15");
            commit7.Message.Returns("Commit 7");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            return commits;
        }
    }
}
