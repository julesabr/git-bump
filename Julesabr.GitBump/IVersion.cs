using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Julesabr.GitBump {
    public interface IVersion {
        private const string RegexPattern = @"^([0-9]+\.){2}[0-9]+(\.[a-zA-z]+\.[0-9]+)?$";

        public const char Separator = '.';
        public const string BlankStringError = "Value cannot be null or empty.";
        public const string PrereleaseBuildIsZeroError = "Prerelease Build number cannot be 0.";

        public const string InvalidPrereleaseBumpError = "Cannot bump prerelease build number when version is not a " +
                                                         "prerelease.";

        public const string InvalidStringFormatError = "'{0}' is not a valid version. All versions must be in a " +
                                                       "semantic version format either 'x.y.z' or 'x.y.z.<branch>.n'.";

        uint Major { get; }
        uint Minor { get; }
        uint Patch { get; }
        string? PrereleaseBranch { get; }
        uint PrereleaseBuild { get; }
        bool IsPrerelease { get; }

        [Pure]
        IVersion BumpMajor();

        [Pure]
        IVersion BumpMinor();

        [Pure]
        IVersion BumpPatch();

        [Pure]
        IVersion BumpPrereleaseBuild();

        [Pure]
        public static IVersion From(uint major, uint minor = 0, uint patch = 0) {
            return new Version(major, minor, patch);
        }

        [Pure]
        public static IVersion From(uint major, uint minor, uint patch, string prereleaseBranch, uint prereleaseBuild) {
            if (string.IsNullOrWhiteSpace(prereleaseBranch))
                throw new ArgumentNullException(nameof(prereleaseBranch), BlankStringError);

            if (prereleaseBuild == 0)
                throw new ArgumentException(PrereleaseBuildIsZeroError);

            return new Version(major, minor, patch, prereleaseBranch, prereleaseBuild, true);
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
                result = From(uint.Parse(revisions[0]), uint.Parse(revisions[1]),
                    uint.Parse(revisions[2]), revisions[3], uint.Parse(revisions[4]));
            else
                result = From(uint.Parse(revisions[0]), uint.Parse(revisions[1]),
                    uint.Parse(revisions[2]));

            return result;
        }
    }
}
