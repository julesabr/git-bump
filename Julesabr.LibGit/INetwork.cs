using JetBrains.Annotations;

namespace Julesabr.LibGit {
    public interface INetwork {
        void PushTags();

        [Pure]
        public static INetwork Create() {
            return new Network();
        }
    }
}
