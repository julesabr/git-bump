using System;

namespace Julesabr.GitBump {
    public sealed class Version : IVersion {
        public uint Major { get; }
        public uint Minor { get; }
        public uint Patch { get; }
        public string PrereleaseBranch { get; }
        public uint PrereleaseBuild { get; }
        public bool IsPrerelease { get; }

        public Version BumpMajor() {
            return new Version(Major + 1, 0, 0, PrereleaseBranch, 1, IsPrerelease);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Version)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Major, Minor, Patch, PrereleaseBranch, PrereleaseBuild);
        }

        private Version(uint major, uint minor, uint patch, string prereleaseBranch = null, uint prereleaseBuild = 1,
            bool isPrerelease = false) {
            Major = major;
            Minor = minor;
            Patch = patch;
            PrereleaseBranch = prereleaseBranch;
            PrereleaseBuild = prereleaseBuild;
            IsPrerelease = isPrerelease;
        }

        private bool Equals(IVersion other) {
            return Major == other.Major &&
                   Minor == other.Minor &&
                   Patch == other.Patch &&
                   PrereleaseBranch == other.PrereleaseBranch &&
                   PrereleaseBuild == other.PrereleaseBuild &&
                   IsPrerelease == other.IsPrerelease;
        }

        public static Version From(uint major, uint minor = 0, uint patch = 0) {
            return new Version(major, minor, patch);
        }

        public static Version From(uint major, uint minor, uint patch, string prereleaseBranch, uint prereleaseBuild) {
            return new Version(major, minor, patch, prereleaseBranch, prereleaseBuild, true);
        }
    }
}