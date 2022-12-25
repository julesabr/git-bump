using CommandLine;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Options {
        [Option('c', "channel", Required = false,
            HelpText =
                "The channel to use in prerelease versions. By default, this is the git branch that is currently checked-out. This is irrelevant if prerelease is false.")]
        public string Channel { get; init; } = "";

        [Option('p', "prefix", Required = false, Default = "v",
            HelpText =
                "The git tag prefix. Git tag names are always going to be the version with the prefix appended at the beginning.")]
        public string Prefix { get; init; } = "v";
        
        [Option('s', "suffix", Required = false,
            HelpText =
                "The git tag suffix. Git tag names are always going to be the version with the suffix appended at the end.")]
        public string Suffix { get; init; } = "";
        
        [Option("version-output", Required = false,
            HelpText =
                "File path to output the new version to. By default, don't output to file. The path could be absolute or relative to the current working directory.")]
        public string VersionOutput { get; init; } = "";

        [Option(Required = false, Default = false,
            HelpText =
                "Toggle this version bump as a prerelease.")]
        public bool Prerelease { get; init; }

        [Option(Required = false, Default = false,
            HelpText =
                "Create a git annotated tag and push the tag to git.")]
        public bool Push { get; init; }

        [Option('t', "tag", Required = false, Default = false,
            HelpText =
                "Use the prefix and suffix to create a git annotated tag from the new version. This will only apply the tag but will not push it. This is irrelevant if push is true.")]
        public bool Tag { get; init; }

        [Option('h', "help", Required = false,
            HelpText =
                "Display this help screen.")]
        public bool Help { get; init; }
        
        [Option('v', "version", Required = false,
            HelpText =
                "Display version information.")]
        public bool Version { get; init; }

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
