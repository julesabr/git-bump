using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitTagTest {
        #region Comparisons

        [Test]
        public void LessThan_WhenLeftPrefixIsLessThanTheRightPrefix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "b", "x");
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftPrefixIsGreaterThanTheRightPrefix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "b", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftVersionIsLessThanTheRightVersion_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(2), "a", "x");
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftVersionIsGreaterThanTheRightVersion_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(2), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenLeftSuffixIsLessThanTheRightSuffix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "y");
            (left < right).Should().BeTrue();
        }

        [Test]
        public void LessThan_WhenLeftSuffixIsGreaterThanTheRightSuffix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "y");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left < right).Should().BeFalse();
        }

        [Test]
        public void LessThan_WhenGitTagsAreEqual_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left < right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftPrefixIsLessThanTheRightPrefix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "b", "x");
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftPrefixIsGreaterThanTheRightPrefix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "b", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftVersionIsLessThanTheRightVersion_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(2), "a", "x");
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftVersionIsGreaterThanTheRightVersion_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(2), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenLeftSuffixIsLessThanTheRightSuffix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "y");
            (left > right).Should().BeFalse();
        }

        [Test]
        public void GreaterThan_WhenLeftSuffixIsGreaterThanTheRightSuffix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "y");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left > right).Should().BeTrue();
        }

        [Test]
        public void GreaterThan_WhenGitTagsAreEqual_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left > right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrefixIsLessThanTheRightPrefix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "b", "x");
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftPrefixIsGreaterThanTheRightPrefix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "b", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftVersionIsLessThanTheRightVersion_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(2), "a", "x");
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftVersionIsGreaterThanTheRightVersion_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(2), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftSuffixIsLessThanTheRightSuffix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "y");
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqualTo_WhenLeftSuffixIsGreaterThanTheRightSuffix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "y");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left <= right).Should().BeFalse();
        }

        [Test]
        public void LessThanOrEqualTo_WhenGitTagsAreEqual_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left <= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPrefixIsLessThanTheRightPrefix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "b", "x");
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftPrefixIsGreaterThanTheRightPrefix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "b", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftVersionIsLessThanTheRightVersion_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(2), "a", "x");
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftVersionIsGreaterThanTheRightVersion_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(2), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftSuffixIsLessThanTheRightSuffix_ThenReturnFalse() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "y");
            (left >= right).Should().BeFalse();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenLeftSuffixIsGreaterThanTheRightSuffix_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "y");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left >= right).Should().BeTrue();
        }

        [Test]
        public void GreaterThanOrEqualTo_WhenGitTagsAreEqual_ThenReturnTrue() {
            IGitTag left = IGitTag.Create(IVersion.From(1), "a", "x");
            IGitTag right = IGitTag.Create(IVersion.From(1), "a", "x");
            (left >= right).Should().BeTrue();
        }

        #endregion
        
        [Test]
        public void Create_GivenVersionObjectIsNull_ThenThrowArgumentNullException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IGitTag.Create((IVersion)null!);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'version')");
        }

        [Test]
        public void Create_GivenGitTagAsValidString_PrefixIsNotNull_AndSuffixIsNotNull_ThenReturnGitTagObject() {
            IGitTag.Create("pre1.2.3-post", "pre", "-post")
                .Should()
                .Be(IGitTag.Create(IVersion.From(1, 2, 3), "pre", "-post"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Create_GivenGitTagAsValidString_AndPrefixIsNullOrEmpty_ThenReturnGitTagObject(string prefix) {
            IGitTag.Create("1.2.3-post", prefix, "-post")
                .Should()
                .Be(IGitTag.Create(IVersion.From(1, 2, 3), prefix, "-post"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Create_GivenGitTagAsValidString_AndSuffixIsNullOrEmpty_ThenReturnGitTagObject(string suffix) {
            IGitTag.Create("pre1.2.3", "pre", suffix)
                .Should()
                .Be(IGitTag.Create(IVersion.From(1, 2, 3), "pre", suffix));
        }

        [Test]
        public void Create_GivenGitTagAsStringWithMissingPrefix_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IGitTag.Create("1.2.3", "pre");
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingPrefixError, "1.2.3", "pre"));
        }

        [Test]
        public void Create_GivenGitTagAsStringWithMissingSuffix_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IGitTag.Create("1.2.3", "", "-post");
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingSuffixError, "1.2.3", "-post"));
        }

        [Test]
        public void Create_GivenGitTagAsStringWithInvalidVersion_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once StringLiteralTypo
            Action action = () => IGitTag.Create("prefoo-post", "pre", "-post");
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IVersion.InvalidStringFormatError, "foo"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Create_GivenGitTagAsStringIsNullOrWhitespace_ThenThrowArgumentNullException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => IGitTag.Create(value);
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage(IGitTag.BlankStringError + " (Parameter 'value')");
        }

        [Test]
        [TestCase((ushort)3u, (ushort)2u, (ushort)1u, "p", "s", "p3.2.1s")]
        [TestCase((ushort)6u, (ushort)8u, (ushort)2u, "v", "", "v6.8.2")]
        [TestCase((ushort)6u, (ushort)8u, (ushort)2u, "", "u", "6.8.2u")]
        [TestCase((ushort)6u, (ushort)8u, (ushort)2u, "v", null, "v6.8.2")]
        [TestCase((ushort)6u, (ushort)8u, (ushort)2u, null, "u", "6.8.2u")]
        public void ToString_WhenVersionIsNotPrerelease_ThenReturnGitTagAsStringInReleaseFormat(
            ushort major,
            ushort minor,
            ushort patch,
            string? prefix,
            string? suffix,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch);
            IGitTag gitTag = IGitTag.Create(version, prefix, suffix);
            gitTag.ToString().Should().Be(value);
        }

        [Test]
        [TestCase((ushort)5u, (ushort)12u, (ushort)1u, "alpha", (ushort)3u, "p", "s",
            "p5.12.1.alpha.3s")]
        [TestCase((ushort)1u, (ushort)6u, (ushort)2u, "staging", (ushort)15u, "v", "",
            "v1.6.2.staging.15")]
        [TestCase((ushort)1u, (ushort)6u, (ushort)2u, "staging", (ushort)15u, "", "u",
            "1.6.2.staging.15u")]
        [TestCase((ushort)1u, (ushort)6u, (ushort)2u, "staging", (ushort)15u, "v", null,
            "v1.6.2.staging.15")]
        [TestCase((ushort)1u, (ushort)6u, (ushort)2u, "staging", (ushort)15u, null, "u",
            "1.6.2.staging.15u")]
        public void ToString_WhenVersionIsPrerelease_ThenReturnGitTagAsStringInPrereleaseFormat(
            ushort major,
            ushort minor,
            ushort patch,
            string prereleaseBranch,
            ushort prereleaseNumber,
            string? prefix,
            string? suffix,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch, prereleaseBranch, prereleaseNumber);
            IGitTag gitTag = IGitTag.Create(version, prefix, suffix);
            gitTag.ToString().Should().Be(value);
        }
    }
}
