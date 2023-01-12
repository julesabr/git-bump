using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Tag {
        public virtual string Name { get; }
        public virtual bool IsAnnotated { get; }
        public virtual Commit Target { get; }

        internal Tag(string name, bool isAnnotated, Commit target) {
            Name = name;
            IsAnnotated = isAnnotated;
            Target = target;
        }

        protected Tag() {
            Name = null!;
            IsAnnotated = false;
            Target = null!;
        }
    }
}
