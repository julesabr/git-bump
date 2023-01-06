using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitTagFactoryTest {
        private IVersion.Factory versionFactory = null!;
        private IGitTag.Factory gitTagFactory = null!;

        [SetUp]
        public void Setup() {
            versionFactory = Substitute.For<IVersion.Factory>();
            gitTagFactory = new IGitTag.Factory(versionFactory);
            
            versionFactory.Create("1.2.3").Returns(new Version(1, 2, 3));
        }
        
        [Test]
        public void Create_GivenVersionIsNotNull_ThenReturnGitTagObject() {
            gitTagFactory.Create(new Version(1, 2, 3), "pre", "-post")
                .Should()
                .Be(new GitTag(new Version(1, 2, 3), "pre", "-post"));
        }

        [Test]
        public void Create_GivenGitTagAsValidString_PrefixIsNotNull_AndSuffixIsNotNull_ThenReturnGitTagObject() {
            gitTagFactory.Create("pre1.2.3-post", "pre", "-post")
                .Should()
                .Be(new GitTag(new Version(1, 2, 3), "pre", "-post"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Create_GivenGitTagAsValidString_AndPrefixIsNullOrEmpty_ThenReturnGitTagObject(string prefix) {
            gitTagFactory.Create("1.2.3-post", prefix, "-post")
                .Should()
                .Be(new GitTag(new Version(1, 2, 3), prefix, "-post"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Create_GivenGitTagAsValidString_AndSuffixIsNullOrEmpty_ThenReturnGitTagObject(string suffix) {
            gitTagFactory.Create("pre1.2.3", "pre", suffix)
                .Should()
                .Be(new GitTag(new Version(1, 2, 3), "pre", suffix));
        }

        [Test]
        public void Create_GivenGitTagAsStringWithMissingPrefix_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => gitTagFactory.Create("1.2.3", "pre", "");
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingPrefixError, "1.2.3", "pre"));
        }

        [Test]
        public void Create_GivenGitTagAsStringWithMissingSuffix_ThenThrowArgumentException() {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => gitTagFactory.Create("1.2.3", "", "-post");
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingSuffixError, "1.2.3", "-post"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Create_GivenGitTagAsStringIsNullOrWhitespace_ThenThrowArgumentNullException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => gitTagFactory.Create(value, "", "");
            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage(IGitTag.BlankStringError + " (Parameter 'value')");
        }
    }
}
