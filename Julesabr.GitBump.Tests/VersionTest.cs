using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class VersionTest {
        [Test]
        public void Bump_GivenReleaseTypeIsNone_ThenReturnSameVersion() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.Bump(ReleaseType.None);
            result.Should().Be(IVersion.From(2, 1, 3));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsMajor_AndNotPrerelease_ThenReturnVersionWhereMajorIncreasedBy1AndBothMinorAndPatchAreResetTo0() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.Bump(ReleaseType.Major);
            result.Should().Be(IVersion.From(3));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsMajor_AndIsPrerelease_ThenReturnVersionWhereMajorIncreasedBy1_MinorAndPatchIsResetTo0_AndPrereleaseNumberIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.Bump(ReleaseType.Major);
            result.Should().Be(IVersion.From(2, 0, 0, "dev", 1));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsMinor_AndNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_AndPatchIsResetTo0() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.Bump(ReleaseType.Minor);
            result.Should().Be(IVersion.From(2, 2));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsMinor_AndIsPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_PatchIsResetTo0_AndPrereleaseNumberIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.Bump(ReleaseType.Minor);
            result.Should().Be(IVersion.From(1, 2, 0, "dev", 1));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsPatch_AndNotPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_AndPatchIsIncreasedBy1() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.Bump(ReleaseType.Patch);
            result.Should().Be(IVersion.From(2, 1, 4));
        }

        [Test]
        public void
            Bump_GivenReleaseTypeIsPatch_AndIsPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_PatchIsIncreasedBy1_AndPrereleaseNumberIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.Bump(ReleaseType.Patch);
            result.Should().Be(IVersion.From(1, 1, 4, "dev", 1));
        }

        [Test]
        public void
            BumpMajor_WhenNotPrerelease_ThenReturnVersionWhereMajorIncreasedBy1AndBothMinorAndPatchAreResetTo0() {
            IVersion version = IVersion.From(2, 1, 3);
            IVersion result = version.BumpMajor();
            result.Should().Be(IVersion.From(3));
        }

        [Test]
        public void
            BumpMajor_WhenPrerelease_ThenReturnVersionWhereMajorIncreasedBy1_MinorAndPatchIsResetTo0_AndPrereleaseNumberIsResetTo1() {
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
            BumpMinor_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsIncreasedBy1_PatchIsResetTo0_AndPrereleaseNumberIsResetTo1() {
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
            BumpPatch_WhenPrerelease_ThenReturnVersionWhereMajorIsTheSame_MinorIsTheSame_PatchIsIncreasedBy1_AndPrereleaseNumberIsResetTo1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPatch();
            result.Should().Be(IVersion.From(1, 1, 4, "dev", 1));
        }

        [Test]
        public void BumpPrerelease_WhenNotPrerelease_ThenThrowInvalidOperationException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(2, 1, 3).BumpPrerelease();
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(IVersion.InvalidPrereleaseBumpError);
        }

        [Test]
        public void BumpPrerelease_WhenPrerelease_ThenReturnVersionWherePrereleaseNumberIsIncreasedBy1() {
            IVersion version = IVersion.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPrerelease();
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
        public void
            From_GivenPrereleaseVersionAsSeparateComponents_AndPrereleaseNumberIs0_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IVersion.From(1, 0, 0, "dev", 0);
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(IVersion.PrereleaseIsZeroError);
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
            ushort prereleaseNumber
        ) {
            IVersion.From(value).Should().Be(IVersion.From(major, minor, patch, prereleaseBranch, prereleaseNumber));
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
            ushort prereleaseNumber,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch, prereleaseBranch, prereleaseNumber);
            version.ToString().Should().Be(value);
        }

        #region Comparisons

        [Test]
        public void LessThan_WhenLeftMajorIsLessThanTheRightMajor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(2, 1, 1);
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftMajorIsGreaterThanTheRightMajor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(0, 1, 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftMinorIsLessThanTheRightMinor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 2, 1);
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftMinorIsGreaterThanTheRightMinor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 0, 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftPatchIsLessThanTheRightPatch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1, 2);
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftPatchIsGreaterThanTheRightPatch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftPrereleaseBranchIsLessThanTheRightPrereleaseBranch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "alpha", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftPrereleaseBranchIsGreaterThanTheRightPrereleaseBranch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "alpha", 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftPrereleaseNumberIsLessThanTheRightPrereleaseNumber_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 2);
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftPrereleaseNumberIsGreaterThanTheRightPrereleaseNumber_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 2);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenVersionsAreEqual_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left < right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftMajorIsLessThanTheRightMajor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(2, 1, 1);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftMajorIsGreaterThanTheRightMajor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(0, 1, 1);
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftMinorIsLessThanTheRightMinor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 2, 1);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftMinorIsGreaterThanTheRightMinor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 0, 1);
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftPatchIsLessThanTheRightPatch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1, 2);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftPatchIsGreaterThanTheRightPatch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1);
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftPrereleaseBranchIsLessThanTheRightPrereleaseBranch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "alpha", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftPrereleaseBranchIsGreaterThanTheRightPrereleaseBranch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "alpha", 1);
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftPrereleaseNumberIsLessThanTheRightPrereleaseNumber_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 2);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftPrereleaseNumberIsGreaterThanTheRightPrereleaseNumber_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 2);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenVersionsAreEqual_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left > right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftMajorIsLessThanTheRightMajor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(2, 1, 1);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftMajorIsGreaterThanTheRightMajor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(0, 1, 1);
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftMinorIsLessThanTheRightMinor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 2, 1);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftMinorIsGreaterThanTheRightMinor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 0, 1);
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPatchIsLessThanTheRightPatch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1, 2);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPatchIsGreaterThanTheRightPatch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1);
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrereleaseBranchIsLessThanTheRightPrereleaseBranch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "alpha", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrereleaseBranchIsGreaterThanTheRightPrereleaseBranch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "alpha", 1);
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrereleaseNumberIsLessThanTheRightPrereleaseNumber_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 2);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrereleaseNumberIsGreaterThanTheRightPrereleaseNumber_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 2);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenVersionsAreEqual_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftMajorIsLessThanTheRightMajor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(2, 1, 1);
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftMajorIsGreaterThanTheRightMajor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(0, 1, 1);
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftMinorIsLessThanTheRightMinor_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 2, 1);
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftMinorIsGreaterThanTheRightMinor_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 0, 1);
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPatchIsLessThanTheRightPatch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1, 2);
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPatchIsGreaterThanTheRightPatch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1);
            IVersion right = IVersion.From(1, 1);
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPrereleaseBranchIsLessThanTheRightPrereleaseBranch_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "alpha", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void
            GreaterThanOrEqualTo_WhenLeftPrereleaseBranchIsGreaterThanTheRightPrereleaseBranch_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "alpha", 1);
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPrereleaseNumberIsLessThanTheRightPrereleaseNumber_ThenReturnFalse() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 2);
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void
            GreaterThanOrEqualTo_WhenLeftPrereleaseNumberIsGreaterThanTheRightPrereleaseNumber_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 2);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenVersionsAreEqual_ThenReturnTrue() {
            IVersion left = IVersion.From(1, 1, 1, "beta", 1);
            IVersion right = IVersion.From(1, 1, 1, "beta", 1);
            (left >= right).Should().BeTrue();
        }

        #endregion
    }
}
