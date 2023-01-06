namespace Julesabr.GitBump.Tests {
    internal class Given<TSystem> {
        public static readonly Given<TSystem> Instance = new();

        public TBuilder With<TBuilder>() where TBuilder : new() {
            return new TBuilder();
        }
    }
}
