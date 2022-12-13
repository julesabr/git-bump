using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Branch {
        protected Branch() {
        }

        internal Branch(string name) {
            Name = name;
        }

        public virtual string Name { get; }
    }
}
