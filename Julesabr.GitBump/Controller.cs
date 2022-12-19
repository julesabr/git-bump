using System;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Controller {
        public const string ReturnNone = "No Bump";
        
        private readonly IRepository repository;

        public Controller(IRepository repository) {
            this.repository = repository;
        }

        public ExitCode GitBump(Options options) {
            IGitDetails details = IGitDetails.Create(repository, options);
            IGitTag? newTag = details.BumpTag();

            if (options.Tag && newTag != null) {
                repository.ApplyTag(newTag.ToString(), "");
                // repository.Network.PushTags();
            }

            if (newTag == null)
                Console.WriteLine(ReturnNone);
            else
                Console.WriteLine(newTag.Version);

            return ExitCode.Success;
        }
    }
}
