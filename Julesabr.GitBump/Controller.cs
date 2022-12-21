using System;
using Julesabr.IO;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Controller {
        public const string ReturnNone = "No Bump";
        
        private readonly IRepository repository;
        private readonly FileFactory fileFactory;

        public Controller(IRepository repository, FileFactory fileFactory) {
            this.repository = repository;
            this.fileFactory = fileFactory;
        }

        public ExitCode GitBump(Options options) {
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

            return ExitCode.Success;
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
