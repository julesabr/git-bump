using FluentAssertions;
using NSubstitute;

namespace Julesabr.GitBump.Tests.GitTagModel {
    internal static class Scenarios {
        [Given]
        public static GivenBuilder With(this Given<GitTag> @this) {
            return @this.With<GivenBuilder>();
        }

        [Given]
        public static GivenBuilder AVersion(this GivenBuilder @this) {
            @this.Version = Given.TheStartingVersion
                .ThatReturnsBumpedVersions()
                .ThatReturnsComparisons(Given.TheLesserVersion, Given.TheNextMajorVersion)
                .ThatReturnsEquals(Given.TheLesserVersion)
                .ThatReturnsAsString();
            return @this;
        }

        [Given]
        public static GivenBuilder APrereleaseVersion(this GivenBuilder @this) {
            @this.Version = Given.TheStartingPrereleaseVersion.ThatReturnsPrereleaseBumpedVersion();
            return @this;
        }

        [Given]
        public static GivenBuilder APrefix(this GivenBuilder @this) {
            @this.Prefix = Given.DefaultPrefix;
            return @this;
        }

        [Given]
        public static GivenBuilder ASuffix(this GivenBuilder @this) {
            @this.Suffix = Given.DefaultSuffix;
            return @this;
        }

        [When]
        public static When<GitTag, IGitTag> Bump(this When<GitTag> @this, ReleaseType releaseType) {
            return @this.AddResult(tag => tag.Bump(releaseType));
        }

        [When]
        public static When<GitTag, IGitTag> BumpPrerelease(this When<GitTag> @this) {
            return @this.AddResult(tag => tag.BumpPrerelease());
        }

        [When]
        public static When<GitTag, int> CompareTo(this When<GitTag> @this, IGitTag? other) {
            return @this.AddResult(tag => tag.CompareTo(other));
        }

        [When]
        public static When<GitTag, bool> EqualsForObject(this When<GitTag> @this, object? obj) {
            return @this.AddResult(tag => tag.Equals(obj));
        }

        [When]
        public static When<GitTag, int> GetHashCodeForObject(this When<GitTag> @this) {
            return @this.AddResult(tag => tag.GetHashCode());
        }

        [When]
        public static When<GitTag, string> ToStringForObject(this When<GitTag> @this) {
            return @this.AddResult(tag => tag.ToString());
        }

        [Then]
        public static void TheResultShouldBeAGitTagForVersion(this Then<IGitTag> @this, IVersion version) {
            IGitTag result = @this.TheResult!;

            result.Version.Should().Be(version);
            result.Prefix.Should().Be(Given.DefaultPrefix);
            result.Suffix.Should().Be(Given.DefaultSuffix);
        }

        private static IVersion ThatReturnsBumpedVersions(this IVersion @this) {
            @this.Bump(ReleaseType.Major).Returns(Given.TheNextMajorVersion);
            @this.Bump(ReleaseType.Minor).Returns(Given.TheNextMinorVersion);
            @this.Bump(ReleaseType.Patch).Returns(Given.TheNextPatchVersion);

            return @this;
        }

        private static IVersion ThatReturnsComparisons(this IVersion @this, IVersion lesser, IVersion greater) {
            @this.CompareTo(lesser).Returns(1);
            @this.CompareTo(@this).Returns(0);
            @this.CompareTo(greater).Returns(-1);

            return @this;
        }

        private static IVersion ThatReturnsEquals(this IVersion @this, IVersion unequal) {
            @this.Equals(unequal).Returns(false);
            @this.Equals(@this).Returns(true);

            return @this;
        }

        private static IVersion ThatReturnsAsString(this IVersion @this) {
            @this.ToString().Returns("1.4.2");
            return @this;
        }

        private static IVersion ThatReturnsPrereleaseBumpedVersion(this IVersion @this) {
            @this.BumpPrerelease().Returns(Given.TheNextPrereleaseVersion);
            return @this;
        }
    }
}
