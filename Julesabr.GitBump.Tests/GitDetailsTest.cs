using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitDetailsTest {
        #region Bumping

        #region Release

        [Test]
        public void BumpTag_WhenLatestTagIsNull_AndThisIsNotAPrerelease_ThenReturnFirstReleaseTag() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IEnumerable<Commit> latestCommits = CommitsWithFeature();
            IGitDetails details = IGitDetails.Create(null, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.First, options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainNoSignificantChange_ThenReturnNull() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithNoSignificantChange();
            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .BeNull();
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainACiChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("e24d30b81e487aa28d8bfa574c15c2c074a507d4");
            commit.Message.Returns("ci: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainABuildChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("build: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainAPerfChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("perf: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainARefactorChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("refactor: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainADocsChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("docs: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainARevertChange_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("revert(calc): Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainABugFix_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBugFix();

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainAFeature_ThenReturnTheLatestTagWithAMinorBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithFeature();

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 2), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainABreakingChangeUsingExclamation_ThenReturnTheLatestTagWithAMajorBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingExclamation();

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsNotAPrerelease_AndTheLatestCommitsContainABreakingChangeUsingMessageFooter_ThenReturnTheLatestTagWithAMajorBump() {
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingMessageFooter();

            IGitDetails details = IGitDetails.Create(latestTag, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3), options.Prefix, options.Suffix));
        }

        #endregion

        #region Prerelease

        [Test]
        public void BumpTag_WhenLatestTagIsNull_AndThisIsAPrerelease_ThenReturnFirstPrereleaseTag() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IEnumerable<Commit> latestCommits = CommitsWithFeature();
            IGitDetails details = IGitDetails.Create(null, null, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(
                    IVersion.From(IVersion.First.Major, IVersion.First.Minor, IVersion.First.Patch, options.Branch, 1),
                    options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_AndTheLatestCommitsContainNoSignificantChange_ThenReturnNull() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithNoSignificantChange();
            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .BeNull();
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainACiChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestPrereleaseTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("e24d30b81e487aa28d8bfa574c15c2c074a507d4");
            commit.Message.Returns("ci: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainACiChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestPrereleaseTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("e24d30b81e487aa28d8bfa574c15c2c074a507d4");
            commit.Message.Returns("ci: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABuildChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("build: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABuildChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("build: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAPerfChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("perf: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAPerfChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("perf: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARefactorChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("refactor: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARefactorChange_AndTheBumpedVersionDoesMotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("refactor: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainADocsChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("docs: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainADocsChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("docs: Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARevertChange_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("revert(calc): Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainARevertChange_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IList<Commit> latestCommits = CommitsWithNoSignificantChange();

            Commit commit = Substitute.For<Commit>();
            commit.Sha.Returns("6c6f2ee44a8bc3105e1fa9e01fcd7e99d3313621");
            commit.Message.Returns("revert(calc): Commit 8");
            latestCommits.Add(commit);

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABugFix_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBugFix();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABugFix_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAPatchBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBugFix();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 1, 4, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAFeature_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 2, 0, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithFeature();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 2, 0, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainAFeature_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMinorBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithFeature();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(2, 2, 0, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingExclamation_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingExclamation();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingExclamation_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMajorBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingExclamation();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 1), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingMessageFooter_AndTheBumpedVersionMatchesPrerelease_ThenReturnTheLatestTagWithAPrereleaseBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingMessageFooter();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 6), options.Prefix, options.Suffix));
        }

        [Test]
        public void
            BumpTag_WhenLatestTagIsNotNull_ThisIsAPrerelease_TheLatestCommitsContainABreakingChangeUsingMessageFooter_AndTheBumpedVersionDoesNotMatchPrerelease_ThenReturnTheLatestTagWithAMajorBump() {
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };
            IGitTag latestTag = IGitTag.Create(IVersion.From(2, 1, 3), options.Prefix, options.Suffix);
            IGitTag latestPrereleaseTag =
                IGitTag.Create(IVersion.From(2, 1, 3, options.Branch, 5), options.Prefix, options.Suffix);
            IEnumerable<Commit> latestCommits = CommitsWithBreakingChangeUsingMessageFooter();

            IGitDetails details = IGitDetails.Create(latestTag, latestPrereleaseTag, latestCommits,
                options);

            details.BumpTag()
                .Should()
                .Be(IGitTag.Create(IVersion.From(3, 0, 0, options.Branch, 1), options.Prefix, options.Suffix));
        }

        #endregion

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

        #endregion

        #region Creation

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSeparateCommitsAndThisIsNotAPrerelease_ThenReturnGitDetailsWithLatestTag() {
            IRepository repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);

            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1, 1)));
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSeparateCommitsAndThisIsAPrerelease_ThenReturnGitDetailsWithLatestTagAndLatestPrereleaseTag() {
            IRepository repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);

            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1, 1)));
            gitDetails.LatestPrereleaseTag.Should()
                .Be(IGitTag.Create(IVersion.From(1, 1, 1, "dev", 2)));
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSameCommitAndThisIsNotAPrerelease_ThenReturnGitDetailsWithLatestTag() {
            IRepository repository = RepositoryWithAnnotatedTagsOnSameCommit();
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);

            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1, 1)));
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsOnSameCommitAndThisIsAPrerelease_ThenReturnGitDetailsWithLatestTagAndLatestPrereleaseTag() {
            IRepository repository = RepositoryWithAnnotatedTagsOnSameCommit();
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);

            gitDetails.LatestTag.Should().Be(IGitTag.Create(IVersion.From(1, 1)));
            gitDetails.LatestPrereleaseTag.Should()
                .Be(IGitTag.Create(IVersion.From(1, 1, 1, "dev", 2)));
        }

        [Test]
        public void Create_GivenARepositoryWithNoAnnotatedTags_ThenReturnGitDetailsWithLatestTagAsNull() {
            IRepository repository = RepositoryWithNoAnnotatedTags();
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);

            gitDetails.LatestTag.Should().BeNull();
            gitDetails.LatestPrereleaseTag.Should().BeNull();
        }

        [Test]
        public void
            Create_GivenARepositoryWithAnnotatedTagsAndIsNotAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestTag() {
            IRepository repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);
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
            IRepository repository = RepositoryWithNoAnnotatedTags();
            Options options = new() {
                Prerelease = false,
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);
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
            IRepository repository = RepositoryWithAnnotatedTagsOnSeparateCommits();
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);
            IList<Commit> latestCommits = gitDetails.LatestCommits.ToList();

            latestCommits.Should().NotBeNull();
            latestCommits.Count.Should().Be(1);
            latestCommits.Should().Contain(commit => commit.Message == "Commit 6");
        }

        [Test]
        public void
            Create_GivenARepositoryWithNoAnnotatedTagsAndIsAPrerelease_ThenReturnGitDetailsWithLatestCommitsSinceTheLatestTag() {
            IRepository repository = RepositoryWithNoAnnotatedTags();
            Options options = new() {
                Prerelease = true,
                Branch = "dev",
                Prefix = "v",
                Suffix = ""
            };

            IGitDetails gitDetails = IGitDetails.Create(repository, options);
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

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            return repository;
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

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);
            
            return repository;
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

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);

            return repository;
        }

        #endregion
    }
}
