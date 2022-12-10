using System;
using LibGit2Sharp;

namespace Julesabr.GitBump {
    internal class GitService : IGitService {
        private readonly IRepository repository;

        public GitService(IRepository repository) {
            this.repository = repository;
        }

        public void CreateAnnotatedTag(IGitTag tag) {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            Signature tagger = repository.Config.BuildSignature(DateTimeOffset.Now);
            if (tagger == null)
                throw new InvalidOperationException(IGitService.TaggerNotFoundError);

            Tag t = repository.ApplyTag(tag.ToString(), tagger, "");
            if (t == null)
                throw new OperationFailedException(string.Format(IGitService.ApplyTagFailedError, tag));
        }

        public void PushTag(IGitTag tag) {
            throw new NotImplementedException();
        }
    }
}
