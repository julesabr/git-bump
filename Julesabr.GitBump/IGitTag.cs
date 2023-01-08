using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public partial interface IGitTag : IComparable<IGitTag> {
        public const string BlankStringError = "Value cannot be null or empty.";
        public const string MissingPrefixError = "The git tag '{0}' is missing the given prefix '{1}'.";
        public const string MissingSuffixError = "The git tag '{0}' is missing the given suffix '{1}'.";

        IVersion? Version { get; }
        string? Prefix { get; }
        string? Suffix { get; }
        
        [Pure]
        IGitTag Bump(ReleaseType type, Options options);

        [Pure]
        IGitTag BumpMajor(Options options);

        [Pure]
        IGitTag BumpMinor(Options options);

        [Pure]
        IGitTag BumpPatch(Options options);

        [Pure]
        IGitTag BumpPrerelease(Options options);

        [Pure]
        string ToString();

        [Pure]
        public static IGitTag Empty(Options options) => new GitTag(null, options.Prefix, options.Suffix);

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
