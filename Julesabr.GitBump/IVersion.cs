using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Julesabr.GitBump {
    public interface IVersion : IComparable<IVersion> {
        private const string RegexPattern = @"^([0-9]+\.){2}[0-9]+(\.[a-zA-z]+\.[0-9]+)?$";

        public const char Separator = '.';
        public const string BlankStringError = "Value cannot be null or empty.";
        public const string PrereleaseIsZeroError = "Prerelease number cannot be 0.";

        public const string InvalidPrereleaseBumpError = "Cannot bump prerelease build number when version is not a " +
                                                         "prerelease.";

        public const string InvalidStringFormatError = "'{0}' is not a valid version. All versions must be in a " +
                                                       "semantic version format either 'x.y.z' or 'x.y.z.<branch>.n'.";

        ushort Major { get; }
        ushort Minor { get; }
        ushort Patch { get; }
        string? PrereleaseBranch { get; }
        ushort PrereleaseNumber { get; }
        bool IsPrerelease { get; }

        [Pure]
        IVersion BumpMajor();

        [Pure]
        IVersion BumpMinor();

        [Pure]
        IVersion BumpPatch();

        [Pure]
        IVersion BumpPrerelease();

        [Pure]
        public static IVersion From(ushort major, ushort minor = 0, ushort patch = 0) {
            return new Version(major, minor, patch);
        }

        [Pure]
        public static IVersion From(
            ushort major,
            ushort minor,
            ushort patch,
            string prereleaseBranch,
            ushort prereleaseNumber
        ) {
            if (string.IsNullOrWhiteSpace(prereleaseBranch))
                throw new ArgumentNullException(nameof(prereleaseBranch), BlankStringError);

            if (prereleaseNumber == 0)
                throw new ArgumentException(PrereleaseIsZeroError);

            return new Version(major, minor, patch, prereleaseBranch, prereleaseNumber, true);
        }

        [Pure]
        public static IVersion From(string value) {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), BlankStringError);

            Match match = Regex.Match(value, RegexPattern);
            if (!match.Success)
                throw new ArgumentException(string.Format(InvalidStringFormatError, value));

            IVersion result;

            string[] revisions = value.Split(Separator);
            if (revisions.Length == 5)
                result = From(ushort.Parse(revisions[0]), ushort.Parse(revisions[1]),
                    ushort.Parse(revisions[2]), revisions[3], ushort.Parse(revisions[4]));
            else
                result = From(ushort.Parse(revisions[0]), ushort.Parse(revisions[1]),
                    ushort.Parse(revisions[2]));

            return result;
        }

        public static bool operator <(IVersion? left, IVersion? right) {
            return Comparer<IVersion>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(IVersion? left, IVersion? right) {
            return Comparer<IVersion>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(IVersion? left, IVersion? right) {
            return Comparer<IVersion>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(IVersion? left, IVersion? right) {
            return Comparer<IVersion>.Default.Compare(left, right) >= 0;
        }
    }
}
