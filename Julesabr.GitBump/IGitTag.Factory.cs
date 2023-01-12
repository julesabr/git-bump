using System;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public partial interface IGitTag {
        public class Factory {
            private readonly IVersion.Factory versionFactory;

            public Factory(IVersion.Factory versionFactory) {
                this.versionFactory = versionFactory;
            }

            [Pure]
            public virtual IGitTag Create(string value, Options options) {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), BlankStringError);

                string? prefix = options.Prefix;
                string? suffix = options.Suffix;

                if (!string.IsNullOrWhiteSpace(prefix) && !value.StartsWith(prefix))
                    throw new ArgumentException(string.Format(MissingPrefixError, value, prefix));

                if (!string.IsNullOrWhiteSpace(suffix) && !value.EndsWith(suffix))
                    throw new ArgumentException(string.Format(MissingSuffixError, value, suffix));

                string version = value;

                if (!string.IsNullOrWhiteSpace(prefix))
                    version = version[prefix.Length..];

                if (!string.IsNullOrWhiteSpace(suffix))
                    version = version[..version.IndexOf(suffix, StringComparison.Ordinal)];

                return new GitTag(
                    versionFactory.Create(version),
                    prefix,
                    suffix
                );
            }

            [Pure]
            public virtual IGitTag CreateEmpty(Options options) {
                return new GitTag(versionFactory.CreateEmpty(), options.Prefix, options.Suffix);
            }
        }
    }
}
