using CommandLine;

namespace Julesabr.GitBump {
    public class Options {
        [Option(Required = false, Default = false,
            HelpText = "Output the steps to take without making file changes or git changes.")]
        public bool DryRun { get; set; }

        [Option(Required = false, Default = false,
            HelpText =
                "Toggle this version bump as a prerelease. Prerelease versions will be in the format x.y.z.<branch>.n where 'branch' is the name of the current git branch and n is the prerelease number. Prereleases will bump the prerelease number before bumping the major, minor, or patch.")]
        public bool Prerelease { get; set; }

        [Option(Required = false, Default = "",
            HelpText =
                "The branch to use in prerelease versions. By default, this is the git branch that is currently checked-out. This is irrelevant if prerelease is false. Prerelease versions will be in the format x.y.z.<branch>.n where 'branch' is the name of the current git branch and n is the prerelease number.")]
        public string? Branch { get; set; }

        [Option(Required = false, Default = false,
            HelpText = "Disable the default behavior of creating a new git tag.")]
        public bool NoTag { get; set; }

        [Option(Required = false, Default = false,
            HelpText =
                "Disable the default behavior of pushing the new git tag once created. This is irrelevant when using the '--no-tag' flag.")]
        public bool NoPush { get; set; }

        [Option(Required = false, Default = "v",
            HelpText =
                "The git tag prefix. Git tag names are always going to be the version with the prefix appended at the beginning.")]
        public string? Prefix { get; set; }

        [Option(Required = false, Default = "",
            HelpText =
                "The git tag suffix. Git tag names are always going to be the version with the suffix appended at the end.")]
        public string? Suffix { get; set; }

        [Option(Required = false, Default = "",
            HelpText =
                "Output the new version into a file at the given path. The path could be absolute or relative to the current working directory.")]
        public string? OutputVersion { get; set; }
    }
}
