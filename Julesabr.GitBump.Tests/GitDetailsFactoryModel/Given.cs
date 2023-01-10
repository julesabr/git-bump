using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests.GitDetailsFactoryModel {
    internal static class Given {
        private const string DefaultPrefix = "v";
        private const string DefaultSuffix = "";
        private const string DefaultChannel = "dev";

        public static readonly IGitTag EmptyTag = GitTagSubstitute.Empty(DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag
            ReleaseTag1 = GitTagSubstitute.Create(VersionSubstitute.Create(1, 0, 0), DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag
            ReleaseTag2 = GitTagSubstitute.Create(VersionSubstitute.Create(1, 1, 0), DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag PrereleaseDefaultChannelTag1 =
            GitTagSubstitute.Create(VersionSubstitute.Create(1, 1, 1, DefaultChannel, 1, true), DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag PrereleaseDefaultChannelTag2 =
            GitTagSubstitute.Create(VersionSubstitute.Create(1, 1, 1, DefaultChannel, 2, true), DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag PrereleaseNonDefaultChannelTag =
            GitTagSubstitute.Create(VersionSubstitute.Create(1, 1, 1, "staging", 1, true), DefaultPrefix, DefaultSuffix);

        public static readonly Options ReleaseOptions = new() {
            Prerelease = false,
            Channel = null,
            Prefix = DefaultPrefix,
            Suffix = DefaultSuffix
        };

        public static readonly Options PrereleaseOptions = new() {
            Prerelease = true,
            Channel = DefaultChannel,
            Prefix = DefaultPrefix,
            Suffix = DefaultSuffix
        };
        
        public static Given<IGitDetails.Factory> GitDetailsFactory => Given<IGitDetails.Factory>.Instance;
    }

    internal class GivenBuilder {
        public IGitTag.Factory? GitTagFactory { get; set; }
        public IRepository? Repository { get; set; }
        
        public GivenBuilder And() {
            return this;
        }

        public When<IGitDetails.Factory> When() {
            return new When<IGitDetails.Factory>(new IGitDetails.Factory(GitTagFactory!, Repository!));
        }
    }
}
