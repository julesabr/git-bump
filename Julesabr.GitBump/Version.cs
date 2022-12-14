using System;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    internal sealed class Version : IVersion {
        public Version(
            ushort major,
            ushort minor,
            ushort patch,
            string? prereleaseBranch = null,
            ushort prereleaseNumber = 1,
            bool isPrerelease = false
        ) {
            Major = major;
            Minor = minor;
            Patch = patch;
            PrereleaseBranch = prereleaseBranch;
            PrereleaseNumber = prereleaseNumber;
            IsPrerelease = isPrerelease;
        }

        public ushort Major { get; }
        public ushort Minor { get; }
        public ushort Patch { get; }
        public string? PrereleaseBranch { get; }
        public ushort PrereleaseNumber { get; }
        public bool IsPrerelease { get; }

        [Pure]
        public IVersion Bump(ReleaseType type) {
            return type switch {
                ReleaseType.Patch => BumpPatch(),
                ReleaseType.Minor => BumpMinor(),
                ReleaseType.Major => BumpMajor(),
                _ => this
            };
        }

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
        public IVersion BumpPrerelease() {
            if (!IsPrerelease)
                throw new InvalidOperationException(IVersion.InvalidPrereleaseBumpError);

            return new Version(Major, Minor, Patch, PrereleaseBranch,
                (ushort)(PrereleaseNumber + 1), IsPrerelease);
        }

        [Pure]
        public int CompareTo(IVersion? other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            int majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;

            int minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;

            int patchComparison = Patch.CompareTo(other.Patch);
            if (patchComparison != 0) return patchComparison;

            int prereleaseBranchComparison =
                string.Compare(PrereleaseBranch, other.PrereleaseBranch, StringComparison.Ordinal);
            return prereleaseBranchComparison != 0
                ? prereleaseBranchComparison
                : PrereleaseNumber.CompareTo(other.PrereleaseNumber);
        }

        [Pure]
        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Version)obj);
        }

        [Pure]
        public override int GetHashCode() {
            return HashCode.Combine(Major, Minor, Patch, PrereleaseBranch, PrereleaseNumber);
        }

        [Pure]
        public override string ToString() {
            string result = string.Join(IVersion.Separator, Major, Minor, Patch);
            if (IsPrerelease)
                result = string.Join(IVersion.Separator, result, PrereleaseBranch, PrereleaseNumber);

            return result;
        }

        private bool Equals(IVersion other) {
            return Major == other.Major &&
                   Minor == other.Minor &&
                   Patch == other.Patch &&
                   PrereleaseBranch == other.PrereleaseBranch &&
                   PrereleaseNumber == other.PrereleaseNumber &&
                   IsPrerelease == other.IsPrerelease;
        }
    }
}
