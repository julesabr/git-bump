using System;

namespace Julesabr.LibGit {
    public class OperationFailedException : Exception {
        public OperationFailedException(string message) : base(message) {
        }
    }
}
