namespace Julesabr.GitBump.Tests.GitTagFactoryModel {
    internal static class Given {
        public const string ADefaultPrefix = "v";
        public const string ADefaultSuffix = "x";
        public const string AValueWithNoPrefixOrSuffix = "1.0.1";
        public const string AValueWithDefaultPrefixAndSuffix = "v1.0.1x";
        public const string AValueWithDefaultPrefixAndWrongSuffix = "v1.0.1b";
        public const string AValueWithWrongPrefixAndDefaultSuffix = "a1.0.1x";

        public static readonly Options OptionsWithNoPrefixOrSuffix = new() {
            Prefix = null,
            Suffix = null
        };

        public static readonly Options OptionsWithDefaultPrefixAndSuffix = new() {
            Prefix = ADefaultPrefix,
            Suffix = ADefaultSuffix
        };

        public static readonly IVersion AVersion = VersionSubstitute.Create(1, 0, 1);
        public static readonly IVersion AnEmptyVersion = VersionSubstitute.Create(0, 0, 0);

        public static readonly Given<IGitTag.Factory> AGitTagFactory = Given<IGitTag.Factory>.Instance;
    }

    internal class GivenBuilder {
        public IVersion.Factory? VersionFactory { get; set; }

        public When<IGitTag.Factory> When() {
            return new When<IGitTag.Factory>(new IGitTag.Factory(VersionFactory!));
        }
    }
}
