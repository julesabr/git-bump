using System;
using System.IO;
using Julesabr.IO;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Controller {
        public const string ReturnNone = "No Bump";
        private readonly FileFactory fileFactory;

        private readonly IRepository repository;

        public Controller(IRepository repository, FileFactory fileFactory) {
            this.repository = repository;
            this.fileFactory = fileFactory;
        }

        public ExitCode GitBump(Options options) {
            try {
                GitBumpCore(options);
                return ExitCode.Success;
            } catch (ArgumentNullException e) {
                Console.Error.WriteLine($"git-bump: validation error: {e.Message}");
                return ExitCode.NullArgument;
            } catch (ArgumentException e) {
                Console.Error.WriteLine($"git-bump: validation error: {e.Message}");
                return ExitCode.InvalidArgument;
            } catch (FileNotFoundException e) {
                Console.Error.WriteLine($"git-bump: file not found: {e.Message}");
                return ExitCode.FileNotFound;
            } catch (OperationFailedException e) {
                Console.Error.WriteLine($"git-bump: operation failed: {e.Message}");
                return ExitCode.OperationFailed;
            } catch (Exception e) {
                Console.Error.WriteLine($"git-bump: error: {e.Message}");
                return ExitCode.Fail;
            }
        }

        private void GitBumpCore(Options options) {
            IGitDetails details = IGitDetails.Create(repository, options);
            IGitTag? newTag = details.BumpTag();

            if (newTag == null) {
                WriteToFile(ReturnNone, options);
                Console.WriteLine(ReturnNone);
            } else {
                TagAndPush(newTag.ToString(), "", options);
                WriteToFile(newTag.Version.ToString(), options);
                Console.WriteLine(newTag.Version);
            }
        }

        private void TagAndPush(string tagName, string message, Options options) {
            if (options.Push || options.Tag)
                repository.ApplyTag(tagName, message);

            if (options.Push)
                repository.Network.PushTag(tagName);
        }

        private void WriteToFile(string content, Options options) {
            if (string.IsNullOrEmpty(options.VersionOutput))
                return;

            IText text = fileFactory.Create(options.VersionOutput);
            text.Write(content);
        }
    }
}
