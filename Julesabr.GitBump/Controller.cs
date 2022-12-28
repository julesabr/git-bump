using System;
using System.ComponentModel.DataAnnotations;
using CommandDotNet;
using JetBrains.Annotations;
using Julesabr.IO;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    [Command(Description = "Show, create, or push the bumped version.")]
    public class Controller {
        public const string ReturnNone = "No Bump";

        private readonly IRepository repository;
        private readonly FileFactory fileFactory;

        public Controller(IRepository repository, FileFactory fileFactory) {
            this.repository = repository;
            this.fileFactory = fileFactory;
        }

        [DefaultCommand]
        [UsedImplicitly]
        public void Execute(IConsole console, Options options) {
            Options defaultedOptions = options.Default(repository);

            TryWriteToFile("version-output", defaultedOptions.VersionOutput);

            IGitDetails details = IGitDetails.Create(repository, defaultedOptions);
            IGitTag? newTag = details.BumpTag();

            if (newTag == null) {
                WriteToFile(ReturnNone, defaultedOptions.VersionOutput);
                console.WriteLine(ReturnNone);
            } else {
                TagAndPush(newTag.ToString(), "", defaultedOptions);
                WriteToFile(newTag.Version.ToString(), defaultedOptions.VersionOutput);
                console.WriteLine(newTag.Version);
            }
        }

        private void TagAndPush(string tagName, string message, Options options) {
            if (options.Push || options.Tag)
                repository.ApplyTag(tagName, message);

            if (options.Push)
                repository.Network.PushTag(tagName);
        }

        private void WriteToFile(string content, string? path) {
            if (string.IsNullOrWhiteSpace(path))
                return;

            IText text = fileFactory.Create(path);
            text.Write(content);
        }

        private void TryWriteToFile(string option, string? path) {
            try {
                WriteToFile("", path);
            } catch (Exception e) {
                throw new ValidationException($"{option} is invalid. {e.Message}");
            }
        }
    }
}
