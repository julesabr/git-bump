namespace Julesabr.GitBump {
    public enum ExitCode {
        Success = 0,
        Fail = 1,
        ValidationError = 10,
        IllegalState = 11,
        FileNotFound = 12,
        OperationFailed = 13,
        Max = 255
    }
}
