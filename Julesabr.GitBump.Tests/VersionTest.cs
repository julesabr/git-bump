using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class VersionTest {
        [Test]
        public void BumpMajor_WhenNotPrerelease_ThenReturnVersionWhereMajorIncreasedBy1AndBothMinorAndPatchAreResetTo0() {
            IVersion version = Version.From(2, 1, 3);
            IVersion result = version.BumpMajor();
            result.Should().Be(Version.From(3));
        }

        [Test]
        public void BumpMajor_WhenPrerelease_ThenReturnVersionWhereMajorIncreasedBy1_MinorAndPatchIsResetTo0_AndPrereleaseBuildIsResetTo1() {
            IVersion version = Version.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpMajor();
            result.Should().Be(Version.From(2, 0, 0, "dev", 1));
        }

        [Test]
        public void BumpMinor_WhenNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_AndPatchIsResetTo0() {
            IVersion version = Version.From(2, 1, 3);
            IVersion result = version.BumpMinor();
            result.Should().Be(Version.From(2, 2));
        }

        [Test]
        public void BumpMinor_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_PatchIsResetTo0_AndPrereleaseBuildIsResetTo1() {
            IVersion version = Version.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpMinor();
            result.Should().Be(Version.From(1, 2, 0, "dev", 1));
        }

        [Test]
        public void BumpPatch_WhenNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_AndPatchIsIncreasedBy1() {
            IVersion version = Version.From(2, 1, 3);
            IVersion result = version.BumpPatch();
            result.Should().Be(Version.From(2, 1, 4));
        }

        [Test]
        public void BumpPatch_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_PatchIsIncreasedBy1_AndPrereleaseBuildIsResetTo1() {
            IVersion version = Version.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPatch();
            result.Should().Be(Version.From(1, 1, 4, "dev", 1));
        }

        [Test]
        public void BumpPrereleaseBuild_WhenNotPrerelease_ThenThrowInvalidOperationException() {
            Action action = () => Version.From(2, 1, 3).BumpPrereleaseBuild();
            action.Should().Throw<InvalidOperationException>()
                .WithMessage(Version.InvalidPrereleaseBumpError);
        }

        [Test]
        public void BumpPrereleaseBuild_WhenPrerelease_ThenReturnVersionWherePrereleaseBuildIsIncreasedBy1() {
            IVersion version = Version.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPrereleaseBuild();
            result.Should().Be(Version.From(1, 1, 3, "dev", 6));
        }

        [Test]
        [TestCase("3.1.2", 3u, 1u, 2u)]
        [TestCase("2.5.13", 2u, 5u, 13u)]
        public void From_GivenReleaseVersionAsValidString_ThenReturnVersionObject(string value, uint major, 
            uint minor, uint patch) {
            Version.From(value).Should().Be(Version.From(major, minor, patch));
        }

        [Test]
        [TestCase("7.2.1.dev.1", 7u, 2u, 1u, "dev", 1u)]
        [TestCase("5.9.6.beta.5", 5u, 9u, 6u, "beta", 5u)]
        public void From_GivenPrereleaseVersionAsValidString_ThenReturnVersionObject(string value, uint major, 
            uint minor, uint patch, string prereleaseBranch, uint prereleaseBuild) {
            Version.From(value).Should().Be(Version.From(major, minor, patch, prereleaseBranch, prereleaseBuild));
        }

        [Test]
        public void From_GivenVersionAsNull_ThenThrowArgumentNullException() {
            Action action = () => Version.From(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(Version.BlankStringError + " (Parameter 'value')");
        }
        
        [Test]
        public void From_GivenVersionAsEmpty_ThenThrowArgumentNullException() {
            Action action = () => Version.From(" ");
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(Version.BlankStringError + " (Parameter 'value')");
        }

        [Test]
        [TestCase("1.1")]
        [TestCase("1.2.3.4")]
        [TestCase("1.2.3.dev")]
        [TestCase("1.b.3")]
        [TestCase("foo")]
        public void From_GivenVersionAsInvalidString_ThenThrowArgumentException(string value) {
            Action action = () => Version.From(value);
            action.Should().Throw<ArgumentException>()
                .WithMessage(string.Format(Version.InvalidStringFormatError, value));
        }
    }
}