using System;

namespace Julesabr.GitBump {
    internal sealed class GitTag : IGitTag {
        public IVersion Version { get; }
        public string? Prefix { get; }
        public string? Suffix { get; }

        public GitTag(IVersion version, string? prefix, string? suffix) {
            Version = version;
            Prefix = prefix;
            Suffix = suffix;
        }

        private bool Equals(IGitTag other) {
            return Version.Equals(other.Version) && Prefix == other.Prefix && Suffix == other.Suffix;
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == GetType() && Equals((GitTag)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Prefix, Version);
        }

        public override string ToString() {
            return $"{Prefix}{Version}{Suffix}";
        }
    }
}