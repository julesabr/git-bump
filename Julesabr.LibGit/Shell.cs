using System.Diagnostics;

namespace Julesabr.LibGit {
    internal static class Shell {
        public static string Run(string program, string command) {
            using Process process = new();

            ProcessStartInfo startInfo = new() {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = program,
                Arguments = command
            };

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new OperationFailedException(
                    $"Shell command failed with error (exit code: {process.ExitCode})\n{process.StandardError.ReadToEnd()}");

            return process.StandardOutput.ReadToEnd();
        }
    }
}
