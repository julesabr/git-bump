using CommandLine;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Options {
        [Option(Required = false, Default = "",
            HelpText =
                "The channel to use in prerelease versions. By default, this is the git branch that is currently checked-out. This is irrelevant if prerelease is false.")]
        public string Channel { get; init; } = "";

        [Option(Required = false, Default = "v",
            HelpText =
                "The git tag prefix. Git tag names are always going to be the version with the prefix appended at the beginning.")]
        public string Prefix { get; init; } = "v";

        [Option(Required = false, Default = false,
            HelpText =
                "Toggle this version bump as a prerelease. Prerelease versions will be in the format x.y.z.<channel>.n where 'channel' is the prerelease channel the version is on and n is the prerelease number. Prereleases will bump the major, minor, or patch before bumping the prerelease number.")]
        public bool Prerelease { get; init; }
        
        [Option(Required = false, Default = false,
            HelpText =
                "Create a git annotated tag and push the tag to git.")]
        public bool Push { get; init; }

        [Option(Required = false, Default = "",
            HelpText =
                "The git tag suffix. Git tag names are always going to be the version with the suffix appended at the end.")]
        public string Suffix { get; init; } = "";
        
        [Option(Required = false, Default = false,
            HelpText = "Use the prefix and suffix to create a git annotated tag from the new version. This will only apply the tag but will not push it. This is irrelevant if push is true.")]
        public bool Tag { get; init; }
        
        [Option(Required = false, Default = "",
            HelpText =
                "File path to output the new version to. By default, don't output to file. The path could be absolute or relative to the current working directory.")]
        public string VersionOutput { get; init; } = "";

        public Options Default(IRepository repository) {
            string channel = Channel;

            if (string.IsNullOrEmpty(channel))
                channel = repository.Head.Name;

            return new Options {
                Channel = channel,
                Prefix = Prefix,
                Prerelease = Prerelease,
                Push = Push,
                Suffix = Suffix,
                Tag = Tag,
                VersionOutput = VersionOutput
            };
        }
    }
}
