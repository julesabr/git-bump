using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Commit {
        public virtual string Sha { get; }
        public virtual string Message { get; }
        public virtual string MessageFull { get; }

        internal Commit(string sha, string message, string messageFull) {
            Sha = sha;
            Message = message;
            MessageFull = messageFull;
        }

        protected Commit() {
            Sha = null!;
            Message = null!;
            MessageFull = null!;
        }
    }
}
