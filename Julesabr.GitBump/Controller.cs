using System;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public class Controller {
        private readonly IRepository repository;

        public Controller(IRepository repository) {
            this.repository = repository;
        }

        public ExitCode GitBump(Options options) {
            IGitDetails details = IGitDetails.Create(repository, options);
            IGitTag newTag = details.BumpTag();

            // if (newTag > details.LatestTag) {
            //     repository.ApplyTag(newTag.ToString(), "");
            //     repository.Network.PushTags();
            // }

            Console.WriteLine(newTag.Version);

            return ExitCode.Success;
        }
    }
}
