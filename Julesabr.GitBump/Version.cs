using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Julesabr.GitBump {
    public sealed class Version : IVersion {
        private const char Separator = '.';
        private const string RegexPattern = @"^([0-9]+\.){2}[0-9]+(\.[a-zA-z]+\.[0-9]+)?$";

        public const string BlankStringError = "Value cannot be null or empty.";
        public const string PrereleaseBuildIsZeroError = "Prerelease Build number cannot be 0.";
        public const string InvalidPrereleaseBumpError = "Cannot bump prerelease build number when version is not a prerelease.";
        public const string InvalidStringFormatError = "'{0}' is not a valid version. All versions must be in a semantic version format either 'x.y.z' or 'x.y.z.<branch>.n'.";
        
        public uint Major { get; }
        public uint Minor { get; }
        public uint Patch { get; }
        public string PrereleaseBranch { get; }
        public uint PrereleaseBuild { get; }
        public bool IsPrerelease { get; }

        [Pure]
        public IVersion BumpMajor() {
            return new Version(Major + 1, 0, 0, PrereleaseBranch, 1, IsPrerelease);
        }

        [Pure]
        public IVersion BumpMinor() {
            return new Version(Major, Minor + 1, 0, PrereleaseBranch, 1, IsPrerelease);
        }

        [Pure]
        public IVersion BumpPatch() {
            return new Version(Major, Minor, Patch + 1, PrereleaseBranch, 1, IsPrerelease);
        }
        
        [Pure]
        public IVersion BumpPrereleaseBuild() {
            if (!IsPrerelease)
                throw new InvalidOperationException(InvalidPrereleaseBumpError);

            return new Version(Major, Minor, Patch, PrereleaseBranch, PrereleaseBuild + 1, IsPrerelease);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Version)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Major, Minor, Patch, PrereleaseBranch, PrereleaseBuild);
        }

        public override string ToString() {
            string result = string.Join(Separator, Major, Minor, Patch);
            if (IsPrerelease)
                result = string.Join(Separator, result, PrereleaseBranch, PrereleaseBuild);
            
            return result;
        }

        private Version(uint major, uint minor, uint patch, string prereleaseBranch = null, uint prereleaseBuild = 1,
            bool isPrerelease = false) {
            Major = major;
            Minor = minor;
            Patch = patch;
            PrereleaseBranch = prereleaseBranch;
            PrereleaseBuild = prereleaseBuild;
            IsPrerelease = isPrerelease;
        }

        private bool Equals(IVersion other) {
            return Major == other.Major &&
                   Minor == other.Minor &&
                   Patch == other.Patch &&
                   PrereleaseBranch == other.PrereleaseBranch &&
                   PrereleaseBuild == other.PrereleaseBuild &&
                   IsPrerelease == other.IsPrerelease;
        }

        [Pure]
        public static Version From(uint major, uint minor = 0, uint patch = 0) {
            return new Version(major, minor, patch);
        }

        [Pure]
        public static Version From(uint major, uint minor, uint patch, string prereleaseBranch, uint prereleaseBuild) {
            if (string.IsNullOrWhiteSpace(prereleaseBranch))
                throw new ArgumentNullException(nameof(prereleaseBranch), BlankStringError);

            if (prereleaseBuild == 0)
                throw new ArgumentException(PrereleaseBuildIsZeroError);
            
            return new Version(major, minor, patch, prereleaseBranch, prereleaseBuild, true);
        }

        [Pure]
        public static Version From(string value) {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), BlankStringError);
            
            Match match = Regex.Match(value, RegexPattern);
            if (!match.Success)
                throw new ArgumentException(string.Format(InvalidStringFormatError, value));
            
            Version result;
            
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