namespace Julesabr.LibGit {
    internal class Network : INetwork {
        public void PushTag(string tagName) {
            Shell.Run(string.Format(Shell.GitPushTag, tagName));
        }
    }
}
