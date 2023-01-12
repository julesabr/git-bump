using NUnit.Framework;

namespace Julesabr.GitBump.Tests.GitDetailsFactoryModel {
    public class Tests {
        #region Release

        [Test]
        public void
            Create_WithAGitTagFactoryForReleaseOptions_AndARepositoryThatContainsAnnotatedTags_Then_TheResult_Should_HaveValidLatestTag_HaveEmptyLatestPrereleaseTag_AndHaveCommitsUpToLatestTagAsLatestCommits() {
            Given.GitDetailsFactory
                .With()
                .AGitTagFactoryForOptions(Given.ReleaseOptions)
                .And()
                .ARepositoryThatContainsAnnotatedTags()
                .When()
                .Create(Given.ReleaseOptions)
                .Then()
                .TheResult!
                .ShouldHaveValidLatestTag()
                .And()
                .ShouldHaveEmptyLatestPrereleaseTag()
                .And()
                .ShouldHaveCommitsUpToLatestTagAsLatestCommits();
        }

        [Test]
        public void
            Create_WithAGitTagFactoryForReleaseOptions_AndARepositoryThatContainsNoAnnotatedTags_Then_TheResult_Should_HaveEmptyLatestTag_HaveEmptyLatestPrereleaseTag_AndHaveAllCommitsAsLatestCommits() {
            Given.GitDetailsFactory
                .With()
                .AGitTagFactoryForOptions(Given.ReleaseOptions)
                .And()
                .ARepositoryThatContainsNoAnnotatedTags()
                .When()
                .Create(Given.ReleaseOptions)
                .Then()
                .TheResult!
                .ShouldHaveEmptyLatestTag()
                .And()
                .ShouldHaveEmptyLatestPrereleaseTag()
                .And()
                .ShouldHaveAllCommitsAsLatestCommits();
        }

        #endregion

        #region Prerelease

        [Test]
        public void
            Create_WithAGitTagFactoryForPrereleaseOptions_AndARepositoryThatContainsAnnotatedTags_Then_TheResult_Should_HaveValidLatestTag_HaveValidLatestPrereleaseTag_AndHaveCommitsUpToLatestTagAsLatestCommits() {
            Given.GitDetailsFactory
                .With()
                .AGitTagFactoryForOptions(Given.PrereleaseOptions)
                .And()
                .ARepositoryThatContainsAnnotatedTags()
                .When()
                .Create(Given.PrereleaseOptions)
                .Then()
                .TheResult!
                .ShouldHaveValidLatestTag()
                .And()
                .ShouldHaveValidLatestPrereleaseTag()
                .And()
                .ShouldHaveCommitsUpToLatestTagAsLatestCommits();
        }

        [Test]
        public void
            Create_WithAGitTagFactoryForPrereleaseOptions_AndARepositoryThatContainsNoAnnotatedTags_Then_TheResult_Should_HaveEmptyLatestTag_HaveEmptyLatestPrereleaseTag_AndHaveAllCommitsAsLatestCommits() {
            Given.GitDetailsFactory
                .With()
                .AGitTagFactoryForOptions(Given.PrereleaseOptions)
                .And()
                .ARepositoryThatContainsNoAnnotatedTags()
                .And()
                .When()
                .Create(Given.PrereleaseOptions)
                .Then()
                .TheResult!
                .ShouldHaveEmptyLatestTag()
                .And()
                .ShouldHaveEmptyLatestPrereleaseTag()
                .And()
                .ShouldHaveAllCommitsAsLatestCommits();
        }

        #endregion
    }
}
