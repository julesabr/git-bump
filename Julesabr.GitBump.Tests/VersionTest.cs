using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class VersionTest {
        [Test]
        public void
            BumpMajor_WhenNotPrerelease_ThenReturnVersionWhereMajorIncreasedBy1AndBothMinorAndPatchAreResetTo0() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.BumpMajor();
            result.Should().Be(IVersion.From(3));
        }

        [Test]
        public void
            BumpMajor_WhenPrerelease_ThenReturnVersionWhereMajorIncreasedBy1_MinorAndPatchIsResetTo0_AndPrereleaseBuildIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpMajor();
            result.Should().Be(IVersion.From(2, 0, 0, "dev", 1));
        }

        [Test]
        public void
            BumpMinor_WhenNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_AndPatchIsResetTo0() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.BumpMinor();
            result.Should().Be(IVersion.From(2, 2));
        }

        [Test]
        public void
            BumpMinor_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_PatchIsResetTo0_AndPrereleaseBuildIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpMinor();
            result.Should().Be(IVersion.From(1, 2, 0, "dev", 1));
        }

        [Test]
        public void
            BumpPatch_WhenNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_AndPatchIsIncreasedBy1() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.BumpPatch();
            result.Should().Be(IVersion.From(2, 1, 4));
        }

        [Test]
        public void
            BumpPatch_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_PatchIsIncreasedBy1_AndPrereleaseBuildIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPatch();
            result.Should().Be(IVersion.From(1, 1, 4, "dev", 1));
        }

        [Test]
        public void BumpPrereleaseBuild_WhenNotPrerelease_ThenThrowInvalidOperationException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(2, 1, 3).BumpPrereleaseBuild();
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(IVersion.InvalidPrereleaseBumpError);
        }

        [Test]
        public void BumpPrereleaseBuild_WhenPrerelease_ThenReturnVersionWherePrereleaseBuildIsIncreasedBy1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPrereleaseBuild();
            result.Should().Be(IVersion.From(1, 1, 3, "dev", 6));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void
            From_GivenPrereleaseVersionAsSeparateComponents_AndPrereleaseBranchIsNullOrWhitespace_ThenThrowArgumentNullException(
                string prereleaseBranch
            ) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(1, 0, 0, prereleaseBranch, 1);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage(IVersion.BlankStringError + " (Parameter 'prereleaseBranch')");
        }

        [Test]
        public void From_GivenPrereleaseVersionAsSeparateComponents_AndPrereleaseBuildIs0_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(1, 0, 0, "dev", 0);
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(IVersion.PrereleaseBuildIsZeroError);
        }

        [Test]
        [TestCase("3.1.2", (ushort)3u, (ushort)1u, (ushort)2u)]
        [TestCase("2.5.13", (ushort)2u, (ushort)5u, (ushort)13u)]
        public void From_GivenReleaseVersionAsValidString_ThenReturnVersionObject(
            string value,
            ushort major,
            ushort minor,
            ushort patch
        ) {
            IVersion.From(value).Should().Be(IVersion.From(major, minor, patch));
        }

        [Test]
        [TestCase("7.2.1.dev.1", (ushort)7u, (ushort)2u, (ushort)1u, "dev", (ushort)1u)]
        [TestCase("5.9.6.beta.5", (ushort)5u, (ushort)9u, (ushort)6u, "beta", (ushort)5u)]
        public void From_GivenPrereleaseVersionAsValidString_ThenReturnVersionObject(
            string value,
            ushort major,
            ushort minor,
            ushort patch,
            string prereleaseBranch,
            ushort prereleaseBuild
        ) {
            IVersion.From(value).Should().Be(IVersion.From(major, minor, patch, prereleaseBranch, prereleaseBuild));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void From_GivenVersionAsNullOrWhitespace_ThenThrowArgumentNullException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(value);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage(IVersion.BlankStringError + " (Parameter 'value')");
        }

        [Test]
        [TestCase("1.1")]
        [TestCase("1.2.3.4")]
        [TestCase("1.2.3.dev")]
        [TestCase("1.b.3")]
        [TestCase("foo")]
        public void From_GivenVersionAsInvalidString_ThenThrowArgumentException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(value);
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IVersion.InvalidStringFormatError, value));
        }

        [Test]
        [TestCase((ushort)3u, (ushort)2u, (ushort)1u, "3.2.1")]
        [TestCase((ushort)6u, (ushort)8u, (ushort)2u, "6.8.2")]
        public void ToString_WhenVersionIsNotPrerelease_ThenReturnAsStringInReleaseFormat(
            ushort major,
            ushort minor,
            ushort patch,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch);
            version.ToString().Should().Be(value);
        }

        [Test]
        [TestCase((ushort)5u, (ushort)12u, (ushort)1u, "alpha", (ushort)3u, "5.12.1.alpha.3")]
        [TestCase((ushort)1u, (ushort)6u, (ushort)2u, "staging", (ushort)15u, "1.6.2.staging.15")]
        public void ToString_WhenVersionIsPrerelease_ThenReturnAsStringInPrereleaseFormat(
            ushort major,
            ushort minor,
            ushort patch,
            string prereleaseBranch,
            ushort prereleaseBuild,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch, prereleaseBranch, prereleaseBuild);
            version.ToString().Should().Be(value);
        }
    }
}
