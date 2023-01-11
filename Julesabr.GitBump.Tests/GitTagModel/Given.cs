using System;

namespace Julesabr.GitBump.Tests.GitTagModel {
    internal static class Given {
        public const string DefaultPrefix = "v";
        public const string DefaultSuffix = "x";

        public static readonly IVersion TheLesserVersion = VersionSubstitute.Create(1, 3, 2);
        public static readonly IVersion TheStartingVersion = VersionSubstitute.Create(1, 4, 2);
        public static readonly IVersion TheNextPatchVersion = VersionSubstitute.Create(1, 4, 3);
        public static readonly IVersion TheNextMinorVersion = VersionSubstitute.Create(1, 5, 0);
        public static readonly IVersion TheNextMajorVersion = VersionSubstitute.Create(2, 0, 0);

        public static readonly IVersion TheStartingPrereleaseVersion =
            VersionSubstitute.Create(1, 4, 2, "dev", 5, true);
        public static readonly IVersion TheNextPrereleaseVersion =
            VersionSubstitute.Create(1, 4, 2, "dev", 6, true);
        
        public static readonly IGitTag TheTagWithLesserPrefix = new GitTag(TheStartingVersion, "u", DefaultSuffix);
        public static readonly IGitTag TheTagWithGreaterPrefix = new GitTag(TheStartingVersion, "x", DefaultSuffix);
        public static readonly IGitTag TheTagWithLesserVersion =
            new GitTag(TheLesserVersion, DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag TheTagWithGreaterVersion =
            new GitTag(TheNextMajorVersion, DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag TheTagWithLesserSuffix = new GitTag(TheStartingVersion, DefaultPrefix, "v");
        public static readonly IGitTag TheTagWithGreaterSuffix = new GitTag(TheStartingVersion, DefaultPrefix, "z");
        public static readonly IGitTag TheStartingTag = new GitTag(TheStartingVersion, DefaultPrefix, DefaultSuffix);

        public static Given<GitTag> AGitTag => Given<GitTag>.Instance;
    }

    internal class GivenBuilder {
        public IVersion? Version { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        
        public GivenBuilder And() {
            return this;
        }

        public When<GitTag> When() {
            return new When<GitTag>(new GitTag(Version!, Prefix, Suffix));
        }
        
        public When<GitTag, TResult> When<TResult>(Func<GitTag, TResult> action) {
            GitTag systemUnderTest = new(Version!, Prefix, Suffix);
            return new When<GitTag, TResult>(systemUnderTest, action(systemUnderTest));
        }
    }
}
