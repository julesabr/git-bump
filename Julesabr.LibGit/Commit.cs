using JetBrains.Annotations;

namespace Julesabr.LibGit {
    [PublicAPI]
    public class Commit {
        protected Commit() {
            Sha = null!;
            Message = null!;
            MessageFull = null!;
        }

        internal Commit(string sha, string message, string messageFull) {
            Sha = sha;
            Message = message;
            MessageFull = messageFull;
        }

        public virtual string Sha { get; }
        public virtual string Message { get; }
        public virtual string MessageFull { get; }
    }
}
