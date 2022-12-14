using System;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    internal class GitService : IGitService {
        private readonly IRepository repository;

        public GitService(IRepository repository) {
            this.repository = repository;
        }

        public void CreateAnnotatedTag(IGitTag tag) {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            repository.ApplyTag(tag.ToString()!, "");
        }

        public void PushTags() {
            repository.Network.PushTags();
        }
    }
}
