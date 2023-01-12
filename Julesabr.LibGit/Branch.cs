using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Branch {
        public virtual string Name { get; }

        internal Branch(string name) {
            Name = name;
        }

        protected Branch() {
            Name = null!;
        }
    }
}
