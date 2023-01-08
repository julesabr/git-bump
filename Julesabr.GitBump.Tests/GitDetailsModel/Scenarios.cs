using System.Collections.Generic;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    internal static class Scenarios {
        [Given]
        public static GivenBuilder With(this Given<GitDetails> @this) {
            return @this.With<GivenBuilder>();
        }

        [Given]
        public static GivenBuilder OptionsForRelease(this GivenBuilder @this) {
            Options options = new() {
                Prerelease = false,
                Channel = null,
                Prefix = Given.DefaultPrefix,
                Suffix = Given.DefaultSuffix
            };

            @this.Options = options;
            return @this;
        }

        [Given]
        public static GivenBuilder OptionsForPrerelease(this GivenBuilder @this) {
            Options options = new() {
                Prerelease = true,
                Channel = Given.DefaultChannel,
                Prefix = Given.DefaultPrefix,
                Suffix = Given.DefaultSuffix
            };

            @this.Options = options;
            return @this;
        }

        [Given]
        public static GivenBuilder ALatestTag(this GivenBuilder @this) {
            @this.LatestTag = Given.StartingTag
                .ThatReturnsBumpedTags(
                    Given.NextPatchTag,
                    Given.NextMinorTag,
                    Given.NextMajorTag,
                    @this.Options!
                );
            return @this;
        }

        [Given]
        public static GivenBuilder ALatestPrereleaseTagFor(this GivenBuilder @this, IGitTag start, IGitTag nextPrerelease) {
            @this.ALatestTag();
            
            @this.LatestPrereleaseTag = start
                .ThatReturnsPrereleaseBumpedTag(nextPrerelease, @this.Options!)
                .ThatReturnsBumpedTags(
                    Given.NextPatchTagWithoutPrerelease,
                    Given.NextMinorTagWithoutPrerelease,
                    Given.NextMajorTagWithoutPrerelease,
                    @this.Options!
                );

            ReturnsWhichVersionsAreReleaseEqual();

            return @this;
        }

        [Given]
        private static IGitTag ThatReturnsPrereleaseBumpedTag(this IGitTag @this, IGitTag nextPrerelease,
            Options options) {
            @this.BumpPrerelease(options).Returns(nextPrerelease);
            return @this;
        }

        [Given]
        private static IGitTag ThatReturnsBumpedTags(this IGitTag @this, IGitTag nextPatch, IGitTag nextMinor, 
            IGitTag nextMajor, Options options) {
            @this.Bump(ReleaseType.Patch, options).Returns(nextPatch);
            @this.Bump(ReleaseType.Minor, options).Returns(nextMinor);
            @this.Bump(ReleaseType.Major, options).Returns(nextMajor);

            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContain(this GivenBuilder @this, string change) {
            @this.LatestCommits = CommitListThatContains(change);
            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContainNoSignificantChange(this GivenBuilder @this) {
            @this.LatestCommits = CommitListThatContainsNoSignificantChange();
            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContainABugFix(this GivenBuilder @this) {
            @this.LatestCommits = CommitListThatContainsABugFix();
            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContainAFeature(this GivenBuilder @this) {
            @this.LatestCommits = CommitListThatContainsAFeature();
            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContainABreakingChangeUsingExclamation(this GivenBuilder @this) {
            @this.LatestCommits = CommitListThatContainsABreakingChangeUsingExclamation();
            return @this;
        }

        [Given]
        public static GivenBuilder LatestCommitsThatContainABreakingChangeUsingMessageFooter(this GivenBuilder @this) {
            @this.LatestCommits = CommitListThatContainsABreakingChangeUsingMessageFooter();
            return @this;
        }

        [When]
        public static When<GitDetails, IGitTag?> BumpTag(this When<GitDetails> @this) {
            return @this.AddResult(
                @this.SystemUnderTest.BumpTag()
            );
        }

        [Then]
        public static void TheResultShouldBeAGitTagForVersion(this Then<IGitTag?> @this, IVersion version) {
            IGitTag? result = @this.TheResult;

            result?.Version.Should().Be(version);
            result?.Prefix.Should().Be(Given.DefaultPrefix);
            result?.Suffix.Should().Be(Given.DefaultSuffix);
        }

        private static void ReturnsWhichVersionsAreReleaseEqual() {
            Given.NextPatchVersion.IsReleaseEqual(Given.StartingPrereleaseVersion).Returns(false);
            Given.NextPatchVersion.IsReleaseEqual(Given.StartingPatchVersion).Returns(true);
            Given.NextMinorVersion.IsReleaseEqual(Given.StartingPrereleaseVersion).Returns(false);
            Given.NextMinorVersion.IsReleaseEqual(Given.StartingMinorVersion).Returns(true);
            Given.NextMajorVersion.IsReleaseEqual(Given.StartingPrereleaseVersion).Returns(false);
            Given.NextMajorVersion.IsReleaseEqual(Given.StartingMajorVersion).Returns(true);
        }

        private static IEnumerable<Commit> CommitListThatContainsNoSignificantChange() {
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

        private static IEnumerable<Commit> CommitListThatContains(string change) {
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
            commit5.Message.Returns($"{change}: Commit 5");

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

        private static IEnumerable<Commit> CommitListThatContainsABugFix() {
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

        private static IEnumerable<Commit> CommitListThatContainsAFeature() {
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

        private static IEnumerable<Commit> CommitListThatContainsABreakingChangeUsingExclamation() {
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

        private static IEnumerable<Commit> CommitListThatContainsABreakingChangeUsingMessageFooter() {
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
    }
}
