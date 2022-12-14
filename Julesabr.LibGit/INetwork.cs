namespace Julesabr.LibGit {
    public interface INetwork {
        void PushTags();

        public static INetwork Create() {
            return new Network();
        }
    }
}
