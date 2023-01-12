using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests.GitTagModel {
    public class Tests {
        [Test]
        public void ToString_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_Be_CorrectString() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .ToStringForObject()
                .Then()
                .TheResult
                .Should()
                .Be($"{Given.DefaultPrefix}1.4.2{Given.DefaultSuffix}");
        }

        #region Bumping

        [Test]
        public void
            Bump_ForReleaseTypeAsMajor_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeAGitTagForNextMajorVersion() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .Bump(ReleaseType.Major)
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.TheNextMajorVersion);
        }

        [Test]
        public void
            Bump_ForReleaseTypeAsMinor_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeAGitTagForNextMinorVersion() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .Bump(ReleaseType.Minor)
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.TheNextMinorVersion);
        }

        [Test]
        public void
            Bump_ForReleaseTypeAsPatch_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeAGitTagForNextPatchVersion() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .Bump(ReleaseType.Patch)
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.TheNextPatchVersion);
        }

        [Test]
        public void
            BumpPrerelease_WithAPrereleaseVersion_APrefix_AndASuffix_Then_TheResult_Should_BeAGitTagForNextPrereleaseVersion() {
            Given.AGitTag
                .With()
                .APrereleaseVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .BumpPrerelease()
                .Then()
                .TheResultShouldBeAGitTagForVersion(Given.TheNextPrereleaseVersion);
        }

        #endregion

        #region Comparisons

        [Test]
        public void
            CompareTo_ForGitTagWithLesserPrefix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeGreaterThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithLesserPrefix)
                .Then()
                .TheResult
                .Should()
                .BeGreaterThan(0);
        }

        [Test]
        public void
            CompareTo_ForGitTagWithGreaterPrefix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeLessThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithGreaterPrefix)
                .Then()
                .TheResult
                .Should()
                .BeLessThan(0);
        }

        [Test]
        public void
            CompareTo_ForGitTagWithLesserVersion_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeGreaterThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithLesserVersion)
                .Then()
                .TheResult
                .Should()
                .BeGreaterThan(0);
        }

        [Test]
        public void
            CompareTo_ForGitTagWithGreaterVersion_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeLessThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithGreaterVersion)
                .Then()
                .TheResult
                .Should()
                .BeLessThan(0);
        }

        [Test]
        public void
            CompareTo_ForGitTagWithLesserSuffix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeGreaterThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithLesserSuffix)
                .Then()
                .TheResult
                .Should()
                .BeGreaterThan(0);
        }

        [Test]
        public void
            CompareTo_ForGitTagWithGreaterSuffix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeLessThan_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheTagWithGreaterSuffix)
                .Then()
                .TheResult
                .Should()
                .BeLessThan(0);
        }

        [Test]
        public void CompareTo_ForGitTagWithAllEqual_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_Be_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(Given.TheStartingTag)
                .Then()
                .TheResult
                .Should()
                .Be(0);
        }

        [Test]
        public void CompareTo_ForSameGitTag_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_Be_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When(tag => tag.CompareTo(tag))
                .Then()
                .TheResult
                .Should()
                .Be(0);
        }

        [Test]
        public void CompareTo_ForNullGitTag_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_Be_1() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .CompareTo(null)
                .Then()
                .TheResult
                .Should()
                .Be(1);
        }

        #endregion

        #region Equality

        [Test]
        public void
            Equals_ForGitTagWithDifferentPrefix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeFalse() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(Given.TheTagWithLesserPrefix)
                .Then()
                .TheResult
                .Should()
                .BeFalse();
        }

        [Test]
        public void
            Equals_ForGitTagWithDifferentVersion_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeFalse() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(Given.TheTagWithLesserVersion)
                .Then()
                .TheResult
                .Should()
                .BeFalse();
        }

        [Test]
        public void
            Equals_ForGitTagWithDifferentSuffix_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeFalse() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(Given.TheTagWithLesserSuffix)
                .Then()
                .TheResult
                .Should()
                .BeFalse();
        }

        [Test]
        public void Equals_ForGitTagWithAllEqual_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeTrue() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(Given.TheStartingTag)
                .Then()
                .TheResult
                .Should()
                .BeTrue();
        }

        [Test]
        public void Equals_ForObjectThatIsNotAGitTag_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeFalse() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(new object())
                .Then()
                .TheResult
                .Should()
                .BeFalse();
        }

        [Test]
        public void Equals_ForSameGitTag_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeTrue() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix() // ReSharper disable once EqualExpressionComparison
                .When(tag => tag.Equals(tag))
                .Then()
                .TheResult
                .Should()
                .BeTrue();
        }

        [Test]
        public void Equals_ForNullGitTag_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_BeFalse() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .EqualsForObject(null)
                .Then()
                .TheResult
                .Should()
                .BeFalse();
        }

        [Test]
        public void GetHashCode_WithAVersion_APrefix_AndASuffix_Then_TheResult_Should_NotBe_0() {
            Given.AGitTag
                .With()
                .AVersion()
                .And()
                .APrefix()
                .And()
                .ASuffix()
                .When()
                .GetHashCodeForObject()
                .Then()
                .TheResult
                .Should()
                .NotBe(0);
        }

        #endregion
    }
}
