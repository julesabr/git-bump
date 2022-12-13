using Julesabr.LibGit;

namespace Julesabr.GitBump {
    public interface IGitService {
        void CreateAnnotatedTag(IGitTag tag);
        void PushTag(IGitTag tag);

        public static IGitService Create(IRepository repository) {
            return new GitService(repository);
        }
    }
}
