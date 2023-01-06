namespace Julesabr.GitBump.Tests {
    internal class Then<TResult> {
        public TResult? TheResult { get; }

        public Then(TResult? result) {
            TheResult = result;
        }
    }
}
