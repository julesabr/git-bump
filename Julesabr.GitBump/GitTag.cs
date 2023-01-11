using System;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public sealed class GitTag : IGitTag {
        public GitTag(IVersion version, string? prefix, string? suffix) {
            Version = version;
            Prefix = prefix;
            Suffix = suffix;
        }

        public IVersion Version { get; }
        public string? Prefix { get; }
        public string? Suffix { get; }

        public IGitTag Bump(ReleaseType type) {
            return new GitTag(Version.Bump(type), Prefix, Suffix);
        }

        public IGitTag BumpPrerelease() {
            return new GitTag(Version.BumpPrerelease(), Prefix, Suffix);
        }

        [Pure]
        public int CompareTo(IGitTag? other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            int prefixComparison = string.Compare(Prefix, other.Prefix, StringComparison.Ordinal);
            if (prefixComparison != 0) return prefixComparison;

            int versionComparison = Version.CompareTo(other.Version);
            return versionComparison != 0
                ? versionComparison
                : string.Compare(Suffix, other.Suffix, StringComparison.Ordinal);
        }

        [Pure]
        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((GitTag)obj);
        }
        
        [Pure]
        private bool Equals(IGitTag other) {
            return Prefix == other.Prefix && Version.Equals(other.Version) && Suffix == other.Suffix;
        }

        [Pure]
        public override int GetHashCode() {
            return HashCode.Combine(Prefix, Version, Suffix);
        }
        
        [Pure]
        public override string ToString() {
            return $"{Prefix}{Version.ToString()}{Suffix}";
        }
    }
}
