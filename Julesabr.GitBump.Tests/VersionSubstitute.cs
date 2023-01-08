using NSubstitute;

namespace Julesabr.GitBump.Tests {
    internal static class VersionSubstitute {
        public static IVersion Create(
            ushort major, 
            ushort minor, 
            ushort patch, 
            string? prereleaseChannel = null, 
            ushort prereleaseNumber = 1, 
            bool isPrerelease = false
        ) {
            IVersion version = Substitute.For<IVersion>();
            
            version.Major.Returns(major);
            version.Minor.Returns(minor);
            version.Patch.Returns(patch);
            version.PrereleaseChannel.Returns(prereleaseChannel);
            version.PrereleaseNumber.Returns(prereleaseNumber);
            version.IsPrerelease.Returns(isPrerelease);

            return version;
        }
    }
}
