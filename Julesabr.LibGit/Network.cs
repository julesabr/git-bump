namespace Julesabr.LibGit {
    internal class Network : INetwork {
        public void PushTags() {
            Shell.Run(Shell.GitPushTags);
        }
    }
}
