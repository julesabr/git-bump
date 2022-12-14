using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public interface IGitService {
        void CreateAnnotatedTag(IGitTag tag);
        void PushTags();

        public static IGitService Create(IRepository repository) {
            return new GitService(repository);
        }
    }
}
