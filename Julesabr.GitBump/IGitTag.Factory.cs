using System;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public partial interface IGitTag {
        // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
        public class Factory {
            private readonly IVersion.Factory versionFactory;

            [UsedImplicitly]
            public Factory() {
                versionFactory = null!;
            }

            public Factory(IVersion.Factory versionFactory) {
                this.versionFactory = versionFactory;
            }

            [Pure]
            public virtual IGitTag Create(IVersion version, string? prefix, string? suffix) {
                return new GitTag(
                    version,
                    prefix,
                    suffix
                );
            }
            
            [Pure]
            public virtual IGitTag Create(string value, string? prefix, string? suffix) {
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

                return new GitTag(
                    versionFactory.Create(version),
                    prefix,
                    suffix
                );
            }
        }
    }
}
