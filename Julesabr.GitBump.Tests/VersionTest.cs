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
    }
}