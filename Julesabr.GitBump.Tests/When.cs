using System;

namespace Julesabr.GitBump.Tests {
    internal class When<TSystem> {
        private TSystem SystemUnderTest { get; }

        public When(TSystem systemUnderTest) {
            SystemUnderTest = systemUnderTest;
        }

        public When<TSystem, TResult> AddResult<TResult>(Func<TSystem, TResult> action) {
            return new When<TSystem, TResult>(SystemUnderTest, action(SystemUnderTest));
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

    [AttributeUsage(AttributeTargets.Method)]
    internal class WhenAttribute : Attribute {
    }
}
