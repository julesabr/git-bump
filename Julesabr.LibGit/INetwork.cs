using JetBrains.Annotations;

namespace Julesabr.LibGit {
    public interface INetwork {
        void PushTag(string tagName);

        [Pure]
        public static INetwork Create() {
            return new Network();
        }
    }
}
