using System;

namespace Julesabr.GitBump.Tests {
    internal class When<TSystem> {
        protected TSystem SystemUnderTest { get; }

        public When(TSystem systemUnderTest) {
            SystemUnderTest = systemUnderTest;
        }

        public When<TSystem, TResult> AddResult<TResult>(Func<TSystem, TResult> action) {
            return new When<TSystem, TResult>(SystemUnderTest, action);
        }
    }

    internal class When<TSystem, TResult> : When<TSystem> {
        private Func<TSystem, TResult> Action { get; }

        public When(TSystem systemUnderTest, Func<TSystem, TResult> action) : base(systemUnderTest) {
            Action = action;
        }

        public Then<TResult> Then() {
            return new Then<TResult>(Action(SystemUnderTest));
        }

        public Action ThenAction() {
            return () => Action(SystemUnderTest);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal class WhenAttribute : Attribute {
    }
}
