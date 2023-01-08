namespace Julesabr.GitBump.Tests.GitDetailsModel {
    internal static class Given {
        public const string DefaultPrefix = "v";
        public const string DefaultSuffix = "";

        private static readonly IVersion StartingVersion = VersionSubstitute.Create(2, 1, 3);
        public static readonly IVersion NextPatchVersion = VersionSubstitute.Create(2, 1, 4);
        public static readonly IVersion NextMinorVersion = VersionSubstitute.Create(2, 2, 0);
        public static readonly IVersion NextMajorVersion = VersionSubstitute.Create(3, 0, 0);

        public static readonly IGitTag StartingTag =
            GitTagSubstitute.Create(StartingVersion, DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag NextPatchTag =
            GitTagSubstitute.Create(NextPatchVersion, DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag NextMinorTag =
            GitTagSubstitute.Create(NextMinorVersion, DefaultPrefix, DefaultSuffix);
        public static readonly IGitTag NextMajorTag =
            GitTagSubstitute.Create(NextMajorVersion, DefaultPrefix, DefaultSuffix);

        public static Given<GitDetails> GitDetails => Given<GitDetails>.Instance;
    }
}
