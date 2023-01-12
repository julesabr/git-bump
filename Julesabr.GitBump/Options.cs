using CommandDotNet;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Options : IArgumentModel {
        [Option('c', "channel",
            DescriptionLines = new[] {
                "The channel to use in prerelease versions. By default, this is the git branch",
                "  that is currently checked-out. This is irrelevant if prerelease is false."
            })]
        public string? Channel { get; init; }

        [Option('p', "prefix",
            DescriptionLines = new[] {
                "The git tag prefix. Git tag names are always going to be the version with the",
                "  prefix appended at the beginning."
            })]
        public string? Prefix { get; init; } = "v";

        [Option('s', "suffix",
            DescriptionLines = new[] {
                "The git tag suffix. Git tag names are always going to be the version with the",
                "  suffix appended at the end."
            })]
        public string? Suffix { get; init; } = "";

        [Option("version-output",
            DescriptionLines = new[] {
                "File path to output the new version to, if provided. The path could be absolute",
                "  or relative to the current working directory."
            })]
        public string? VersionOutput { get; private init; }

        [Option("prerelease", Description = "Toggle this version bump as a prerelease")]
        public bool Prerelease { get; init; }

        [Option("push", Description = "Create a git annotated tag and push the tag to git")]
        public bool Push { get; private init; }

        [Option('t', "tag",
            DescriptionLines = new[] {
                "Use the prefix and suffix to create a git annotated tag from the new version.",
                "  This will only apply the tag but will not push it. This is irrelevant if push is",
                "  true."
            })]
        public bool Tag { get; private init; }

        public Options Default(IRepository repository) {
            string? channel = Channel;

            if (string.IsNullOrWhiteSpace(channel))
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
