using System;
using System.Diagnostics.Contracts;

namespace Julesabr.GitBump {
    public interface IGitTag {
        public const string BlankStringError = "Value cannot be null or empty.";
        public const string MissingPrefixError = "The git tag '{0}' is missing the given prefix '{1}'.";
        public const string MissingSuffixError = "The git tag '{0}' is missing the given suffix '{1}'.";
        
        IVersion Version { get; }
        string? Prefix { get; }
        string? Suffix { get; }

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
            
            if (!string.IsNullOrEmpty(prefix) && !value.StartsWith(prefix))
                throw new ArgumentException(string.Format(MissingPrefixError, value, prefix));

            if (!string.IsNullOrEmpty(suffix) && !value.EndsWith(suffix))
                throw new ArgumentException(string.Format(MissingSuffixError, value, suffix));

            string version = value;

            if (!string.IsNullOrEmpty(prefix))
                version = version[prefix.Length..];

            if (!string.IsNullOrEmpty(suffix))
                version = version[..version.IndexOf(suffix, StringComparison.Ordinal)];

            return Create(
                IVersion.From(version),
                prefix, 
                suffix
            );
        }
    }
}