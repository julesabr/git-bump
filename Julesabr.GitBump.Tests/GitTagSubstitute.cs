using NSubstitute;

namespace Julesabr.GitBump.Tests {
    internal static class GitTagSubstitute {
        public static IGitTag Empty(string prefix, string suffix) {
            IGitTag gitTag = Substitute.For<IGitTag>();
            
            gitTag.Version.Returns((IVersion?)null);
            gitTag.Prefix.Returns(prefix);
            gitTag.Suffix.Returns(suffix);

            return gitTag;
        }
        
        public static IGitTag Create(IVersion? version, string prefix, string suffix) {
            IGitTag gitTag = Substitute.For<IGitTag>();
            
            gitTag.Version.Returns(version);
            gitTag.Prefix.Returns(prefix);
            gitTag.Suffix.Returns(suffix);

            return gitTag;
        }
    }
}
