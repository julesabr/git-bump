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
                .WithMessage(Version.INVAILD_PRERELEASE_BUMP_ERROR);
        }

        [Test]
        public void BumpPrereleaseBuild_WhenPrerelease_ThenReturnVersionWherePrereleaseBuildIsIncreasedBy1() {
            IVersion version = Version.From(1, 1, 3, "dev", 5);
            IVersion result = version.BumpPrereleaseBuild();
            result.Should().Be(Version.From(1, 1, 3, "dev", 6));
        }
    }
}