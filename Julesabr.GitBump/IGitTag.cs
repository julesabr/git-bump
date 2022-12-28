using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public interface IGitTag : IComparable<IGitTag> {
        public const string BlankStringError = "Value cannot be null or empty.";
        public const string MissingPrefixError = "The git tag '{0}' is missing the given prefix '{1}'.";
        public const string MissingSuffixError = "The git tag '{0}' is missing the given suffix '{1}'.";

        IVersion Version { get; }
        string? Prefix { get; }
        string? Suffix { get; }

        [Pure]
        string ToString();

        [Pure]
        public static IGitTag Create(IVersion version, string? prefix = "v", string? suffix = "") {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            return new GitTag(version, prefix, suffix);
        }

        [Pure]
        public static IGitTag Create(string value, string? prefix = "v", string? suffix = "") {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), BlankStringError);

            if (!string.IsNullOrWhiteSpace(prefix) && !value.StartsWith(prefix))
                throw new ArgumentException(string.Format(MissingPrefixError, value, prefix));

            if (!string.IsNullOrWhiteSpace(suffix) && !value.EndsWith(suffix))
                throw new ArgumentException(string.Format(MissingSuffixError, value, suffix));

            string version = value;

            if (!string.IsNullOrWhiteSpace(prefix))
                version = version[prefix.Length..];

            if (!string.IsNullOrWhiteSpace(suffix))
                version = version[..version.IndexOf(suffix, StringComparison.Ordinal)];

            return Create(
                IVersion.From(version),
                prefix,
                suffix
            );
        }

        [Pure]
        public static bool operator <(IGitTag? left, IGitTag? right) {
            return Comparer<IGitTag>.Default.Compare(left, right) < 0;
        }

        [Pure]
        public static bool operator >(IGitTag? left, IGitTag? right) {
            return Comparer<IGitTag>.Default.Compare(left, right) > 0;
        }

        [Pure]
        public static bool operator <=(IGitTag? left, IGitTag? right) {
            return Comparer<IGitTag>.Default.Compare(left, right) <= 0;
        }

        [Pure]
        public static bool operator >=(IGitTag? left, IGitTag? right) {
            return Comparer<IGitTag>.Default.Compare(left, right) >= 0;
        }
    }
}
