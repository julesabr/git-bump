using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    internal static class Given {
        public const string DefaultPrefix = "v";
        public const string DefaultSuffix = "";
        public const string DefaultChannel = "dev";

        public static readonly IVersion NextPatchVersion = VersionSubstitute.Create(2, 1, 4);
        public static readonly IVersion NextMinorVersion = VersionSubstitute.Create(2, 2, 0);
        public static readonly IVersion NextMajorVersion = VersionSubstitute.Create(3, 0, 0);

        private static readonly IVersion StartingVersion = VersionSubstitute.Create(2, 1, 3);

        public static readonly IGitTag StartingTag =
            GitTagSubstitute.Create(StartingVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextPatchTag =
            GitTagSubstitute.Create(NextPatchVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMinorTag =
            GitTagSubstitute.Create(NextMinorVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMajorTag =
            GitTagSubstitute.Create(NextMajorVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IVersion StartingPrereleaseVersion =
            VersionSubstitute.Create(2, 1, 3, DefaultChannel, 5, true);

        public static readonly IVersion StartingPatchVersion =
            VersionSubstitute.Create(2, 1, 4, DefaultChannel, 5, true);

        public static readonly IVersion StartingMinorVersion =
            VersionSubstitute.Create(2, 2, 0, DefaultChannel, 5, true);

        public static readonly IVersion StartingMajorVersion =
            VersionSubstitute.Create(3, 0, 0, DefaultChannel, 5, true);

        private static readonly IVersion NextPrereleaseVersion =
            VersionSubstitute.Create(2, 1, 3, DefaultChannel, 6, true);

        public static readonly IVersion NextPatchVersionWithPrerelease =
            VersionSubstitute.Create(2, 1, 4, DefaultChannel, 6, true);

        public static readonly IVersion NextMinorVersionWithPrerelease =
            VersionSubstitute.Create(2, 2, 0, DefaultChannel, 6, true);

        public static readonly IVersion NextMajorVersionWithPrerelease =
            VersionSubstitute.Create(3, 0, 0, DefaultChannel, 6, true);

        public static readonly IVersion NextPatchVersionWithoutPrerelease =
            VersionSubstitute.Create(2, 1, 4, DefaultChannel, 1, true);

        public static readonly IVersion NextMinorVersionWithoutPrerelease =
            VersionSubstitute.Create(2, 2, 0, DefaultChannel, 1, true);

        public static readonly IVersion NextMajorVersionWithoutPrerelease =
            VersionSubstitute.Create(3, 0, 0, DefaultChannel, 1, true);

        public static readonly IGitTag StartingPrereleaseTag =
            GitTagSubstitute.Create(StartingPrereleaseVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag StartingPatchTag =
            GitTagSubstitute.Create(StartingPatchVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag StartingMinorTag =
            GitTagSubstitute.Create(StartingMinorVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag StartingMajorTag =
            GitTagSubstitute.Create(StartingMajorVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextPrereleaseTag =
            GitTagSubstitute.Create(NextPrereleaseVersion, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextPatchTagWithPrerelease =
            GitTagSubstitute.Create(NextPatchVersionWithPrerelease, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMinorTagWithPrerelease =
            GitTagSubstitute.Create(NextMinorVersionWithPrerelease, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMajorTagWithPrerelease =
            GitTagSubstitute.Create(NextMajorVersionWithPrerelease, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextPatchTagWithoutPrerelease =
            GitTagSubstitute.Create(NextPatchVersionWithoutPrerelease, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMinorTagWithoutPrerelease =
            GitTagSubstitute.Create(NextMinorVersionWithoutPrerelease, DefaultPrefix, DefaultSuffix);

        public static readonly IGitTag NextMajorTagWithoutPrerelease =
            GitTagSubstitute.Create(NextMajorVersionWithoutPrerelease, DefaultPrefix, DefaultSuffix);


        public static Given<GitDetails> GitDetails => Given<GitDetails>.Instance;
    }

    internal class GivenBuilder {
        public IGitTag? LatestTag { get; set; }
        public IGitTag? LatestPrereleaseTag { get; set; }
        public IEnumerable<Commit>? LatestCommits { get; set; }
        public Options? Options { get; set; }

        public GivenBuilder And() {
            return this;
        }

        public When<GitDetails> When() {
            return new When<GitDetails>(new GitDetails(LatestTag!, LatestPrereleaseTag!, LatestCommits!, Options!));
        }
    }
}
