using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitTagTest {
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
        [TestCase(3u, 2u, 1u, "p", "s", "p3.2.1s")]
        [TestCase(6u, 8u, 2u, "v", "", "v6.8.2")]
        [TestCase(6u, 8u, 2u, "", "u", "6.8.2u")]
        [TestCase(6u, 8u, 2u, "v", null, "v6.8.2")]
        [TestCase(6u, 8u, 2u, null, "u", "6.8.2u")]
        public void ToString_WhenVersionIsNotPrerelease_ThenReturnGitTagAsStringInReleaseFormat(
            uint major,
            uint minor,
            uint patch,
            string? prefix,
            string? suffix,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch);
            IGitTag gitTag = IGitTag.Create(version, prefix, suffix);
            gitTag.ToString().Should().Be(value);
        }

        [Test]
        [TestCase(5u, 12u, 1u, "alpha", 3u, "p", "s",
            "p5.12.1.alpha.3s")]
        [TestCase(1u, 6u, 2u, "staging", 15u, "v", "",
            "v1.6.2.staging.15")]
        [TestCase(1u, 6u, 2u, "staging", 15u, "", "u",
            "1.6.2.staging.15u")]
        [TestCase(1u, 6u, 2u, "staging", 15u, "v", null,
            "v1.6.2.staging.15")]
        [TestCase(1u, 6u, 2u, "staging", 15u, null, "u",
            "1.6.2.staging.15u")]
        public void ToString_WhenVersionIsPrerelease_ThenReturnGitTagAsStringInPrereleaseFormat(
            uint major,
            uint minor,
            uint patch,
            string prereleaseBranch,
            uint prereleaseBuild,
            string? prefix,
            string? suffix,
            string value
        ) {
            IVersion version = IVersion.From(major, minor, patch, prereleaseBranch, prereleaseBuild);
            IGitTag gitTag = IGitTag.Create(version, prefix, suffix);
            gitTag.ToString().Should().Be(value);
        }
    }
}
