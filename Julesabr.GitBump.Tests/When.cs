namespace Julesabr.GitBump.Tests {
    internal class When<TSystem> {
        public TSystem SystemUnderTest { get; }

        public When(TSystem systemUnderTest) {
            SystemUnderTest = systemUnderTest;
        }

        public When<TSystem, TResult> AddResult<TResult>(TResult result) {
            return new When<TSystem, TResult>(SystemUnderTest, result);
        }
    }

    internal class When<TSystem, TResult> : When<TSystem> {
        private TResult Result { get; }
        
        public When(TSystem systemUnderTest, TResult result) : base(systemUnderTest) {
            Result = result;
        }
        
        public Then<TResult> Then() {
            return new Then<TResult>(Result);
        }
    }
}
