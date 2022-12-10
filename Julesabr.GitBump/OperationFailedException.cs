using System;

namespace Julesabr.GitBump {
    public class OperationFailedException : Exception {
        public OperationFailedException(string? message) : base(message) {
        }
    }
}
