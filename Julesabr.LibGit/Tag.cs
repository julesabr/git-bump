using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Tag {
        protected Tag() {
            Name = null!;
            IsAnnotated = false;
            Target = null!;
        }

        internal Tag(string name, bool isAnnotated, Commit target) {
            Name = name;
            IsAnnotated = isAnnotated;
            Target = target;
        }

        public virtual string Name { get; }
        public virtual bool IsAnnotated { get; }
        public virtual Commit Target { get; }
    }
}
