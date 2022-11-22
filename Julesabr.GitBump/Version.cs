using System;
using System.Diagnostics.Contracts;

namespace Julesabr.GitBump {
    internal sealed class Version : IVersion {
        public Version(
            ushort major,
            ushort minor,
            ushort patch,
            string? prereleaseBranch = null,
            ushort prereleaseBuild = 1,
            bool isPrerelease = false
        ) {
            Major = major;
            Minor = minor;
            Patch = patch;
            PrereleaseBranch = prereleaseBranch;
            PrereleaseBuild = prereleaseBuild;
            IsPrerelease = isPrerelease;
        }

        public ushort Major { get; }
        public ushort Minor { get; }
        public ushort Patch { get; }
        public string? PrereleaseBranch { get; }
        public ushort PrereleaseBuild { get; }
        public bool IsPrerelease { get; }

        [Pure]
        public IVersion BumpMajor() {
            return new Version((ushort)(Major + 1), 0, 0, PrereleaseBranch, 1, IsPrerelease);
        }

        [Pure]
        public IVersion BumpMinor() {
            return new Version(Major, (ushort)(Minor + 1), 0, PrereleaseBranch, 1, IsPrerelease);
        }

        [Pure]
        public IVersion BumpPatch() {
            return new Version(Major, Minor, (ushort)(Patch + 1), PrereleaseBranch, 1, IsPrerelease);
        }

        [Pure]
        public IVersion BumpPrereleaseBuild() {
            if (!IsPrerelease)
                throw new InvalidOperationException(IVersion.InvalidPrereleaseBumpError);

            return new Version(Major, Minor, Patch, PrereleaseBranch, (ushort)(PrereleaseBuild + 1), IsPrerelease);
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Version)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Major, Minor, Patch, PrereleaseBranch, PrereleaseBuild);
        }

        public override string ToString() {
            string result = string.Join(IVersion.Separator, Major, Minor, Patch);
            if (IsPrerelease)
                result = string.Join(IVersion.Separator, result, PrereleaseBranch, PrereleaseBuild);

            return result;
        }

        private bool Equals(IVersion other) {
            return Major == other.Major &&
                   Minor == other.Minor &&
                   Patch == other.Patch &&
                   PrereleaseBranch == other.PrereleaseBranch &&
                   PrereleaseBuild == other.PrereleaseBuild &&
                   IsPrerelease == other.IsPrerelease;
        }
    }
}
