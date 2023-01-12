using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    public class Tests {
        #region Release

        [Test]
        public void
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainNoSignificantChange_Then_TheResult_Should_BeNull() {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainNoSignificantChange()
                .When()
                .BumpTag()
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
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainAChange_Then_TheResult_Should_Be_GitTagForNextPatchVersion(
                string change
            ) {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContain(change)
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersion);
        }

        [Test]
        public void
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainABugFix_ThenTheResult_Should_Be_GitTagForNextPatchVersion() {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABugFix()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersion);
        }

        [Test]
        public void
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainAFeature_ThenTheResult_Should_Be_GitTagForNextMinorVersion() {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainAFeature()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMinorVersion);
        }

        [Test]
        public void
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainABreakingChangeUsingExclamation_ThenTheResult_Should_Be_GitTagForNextMajorVersion() {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABreakingChangeUsingExclamation()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersion);
        }

        [Test]
        public void
            BumpTag_WithOptionsForRelease_ALatestTag_AndLatestCommitsThatContainABreakingChangeUsingMessageFooter_ThenTheResult_Should_Be_GitTagForNextMajorVersion() {
            Given.GitDetails
                .With()
                .OptionsForRelease()
                .And()
                .ALatestTag()
                .And()
                .LatestCommitsThatContainABreakingChangeUsingMessageFooter()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersion);
        }

        #endregion

        #region Prerelease

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTag_AndLatestCommitsThatContainNoSignificantChange_ThenTheResult_Should_BeNull() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPatchTag, Given.NextPatchTagWithPrerelease)
                .And()
                .LatestCommitsThatContainNoSignificantChange()
                .When()
                .BumpTag()
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
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsInSameRelease_AndLatestCommitsThatContainAChange_Then_TheResult_Should_Be_GitTagForNextPatchVersionWithPrerelease(
                string change
            ) {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPatchTag, Given.NextPatchTagWithPrerelease)
                .And()
                .LatestCommitsThatContain(change)
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersionWithPrerelease);
        }

        [Test]
        [TestCase("ci")]
        [TestCase("build")]
        [TestCase("perf")]
        [TestCase("refactor")]
        [TestCase("docs")]
        [TestCase("revert")]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsNotInSameRelease_AndLatestCommitsThatContainAChange_Then_TheResult_Should_Be_GitTagForNextPatchVersionWithoutPrerelease(
                string change
            ) {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPrereleaseTag, Given.NextPrereleaseTag)
                .And()
                .LatestCommitsThatContain(change)
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersionWithoutPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsInSameRelease_AndLatestCommitsThatContainABugFix_Then_TheResult_Should_Be_GitTagForNextPatchVersionWithPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPatchTag, Given.NextPatchTagWithPrerelease)
                .And()
                .LatestCommitsThatContainABugFix()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersionWithPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsNotInSameRelease_AndLatestCommitsThatContainABugFix_Then_TheResult_Should_Be_GitTagForNextPatchVersionWithoutPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPrereleaseTag, Given.NextPrereleaseTag)
                .And()
                .LatestCommitsThatContainABugFix()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextPatchVersionWithoutPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsInSameRelease_AndLatestCommitsThatContainAFeature_Then_TheResult_Should_Be_GitTagForNextMinorVersionWithPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingMinorTag, Given.NextMinorTagWithPrerelease)
                .And()
                .LatestCommitsThatContainAFeature()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMinorVersionWithPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsNotInSameRelease_AndLatestCommitsThatContainAFeature_Then_TheResult_Should_Be_GitTagForNextMinorVersionWithoutPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPrereleaseTag, Given.NextPrereleaseTag)
                .And()
                .LatestCommitsThatContainAFeature()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMinorVersionWithoutPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsInSameRelease_AndLatestCommitsThatContainABreakingChangeUsingExclamation_Then_TheResult_Should_Be_GitTagForNextMajorVersionWithPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingMajorTag, Given.NextMajorTagWithPrerelease)
                .And()
                .LatestCommitsThatContainABreakingChangeUsingExclamation()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersionWithPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsNotInSameRelease_AndLatestCommitsThatContainABreakingChangeUsingExclamation_Then_TheResult_Should_Be_GitTagForNextMajorVersionWithoutPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPrereleaseTag, Given.NextPrereleaseTag)
                .And()
                .LatestCommitsThatContainABreakingChangeUsingExclamation()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersionWithoutPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsInSameRelease_AndLatestCommitsThatContainABreakingChangeUsingMessageFooter_Then_TheResult_Should_Be_GitTagForNextMajorVersionWithPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingMajorTag, Given.NextMajorTagWithPrerelease)
                .And()
                .LatestCommitsThatContainABreakingChangeUsingMessageFooter()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersionWithPrerelease);
        }

        [Test]
        public void
            BumpTag_WithOptionsForPrerelease_ALatestPrereleaseTagWhereBumpedLatestTagIsNotInSameRelease_AndLatestCommitsThatContainABreakingChangeUsingMessageFooter_Then_TheResult_Should_Be_GitTagForNextMajorVersionWithoutPrerelease() {
            Given.GitDetails
                .With()
                .OptionsForPrerelease()
                .And()
                .ALatestPrereleaseTagFor(Given.StartingPrereleaseTag, Given.NextPrereleaseTag)
                .And()
                .LatestCommitsThatContainABreakingChangeUsingMessageFooter()
                .When()
                .BumpTag()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.NextMajorVersionWithoutPrerelease);
        }

        #endregion
    }
}
