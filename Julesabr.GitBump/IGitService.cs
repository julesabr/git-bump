using LibGit2Sharp;

namespace Julesabr.GitBump {
    public interface IGitService {
        public const string TaggerNotFoundError =
            "Tagger not found due to missing configuration. Please set your user.name and user.email in the git config.";
        public const string ApplyTagFailedError = "Unable to create tag: {0}";

        void CreateAnnotatedTag(IGitTag tag);
        void PushTag(IGitTag tag);

        public static IGitService Create(IRepository repository) {
            return new GitService(repository);
        }
    }
}
