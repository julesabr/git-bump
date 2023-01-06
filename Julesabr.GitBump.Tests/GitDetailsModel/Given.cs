using NSubstitute;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    internal static class Given {
        public const string DefaultPrefix = "v";
        public const string DefaultSuffix = "";
        public static readonly IVersion StartingVersion = IVersion.From(2, 1, 3);
        public static readonly IVersion NextPatchVersion = IVersion.From(2, 1, 4);
        public static readonly IVersion NextMinorVersion = IVersion.From(2, 2);
        public static readonly IVersion NextMajorVersion = IVersion.From(3);

        public static IVersion AVersion => Substitute.For<IVersion>();
        
        public static IGitTag.Factory AGitTagFactory => Substitute.For<IGitTag.Factory>();
        
        public static Given<GitDetails> GitDetails => Given<GitDetails>.Instance;
    }
}
