using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class VersionFactoryTest {
        private IVersion.Factory versionFactory = null!;

        [SetUp]
        public void Setup() {
            versionFactory = new IVersion.Factory();
        }

        [Test]
        [TestCase("3.1.2", (ushort)3u, (ushort)1u, (ushort)2u)]
        [TestCase("2.5.13", (ushort)2u, (ushort)5u, (ushort)13u)]
        public void Create_GivenReleaseVersionAsValidString_ThenReturnVersionObject(
            string value,
            ushort major,
            ushort minor,
            ushort patch
        ) {
            versionFactory.Create(value).Should().Be(new Version(major, minor, patch));
        }

        [Test]
        [TestCase("7.2.1.dev.1", (ushort)7u, (ushort)2u, (ushort)1u, "dev", (ushort)1u)]
        [TestCase("5.9.6.beta.5", (ushort)5u, (ushort)9u, (ushort)6u, "beta", (ushort)5u)]
        public void Create_GivenPrereleaseVersionAsValidString_ThenReturnVersionObject(
            string value,
            ushort major,
            ushort minor,
            ushort patch,
            string prereleaseBranch,
            ushort prereleaseNumber
        ) {
            versionFactory.Create(value)
                .Should()
                .Be(new Version(major, minor, patch, prereleaseBranch, prereleaseNumber));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_GivenVersionAsNullOrWhitespace_ThenThrowArgumentNullException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => versionFactory.Create(value);
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
        public void Create_GivenVersionAsInvalidString_ThenThrowArgumentException(string value) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => versionFactory.Create(value);
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IVersion.InvalidStringFormatError, value));
        }
    }
}
