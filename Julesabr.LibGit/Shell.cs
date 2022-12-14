using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Julesabr.LibGit {
    internal static class Shell {
#if OS_WINDOWS
        private const string Program = "Git\\bin\\bash.exe";
#else
        private const string Program = "bash";
#endif
        private const int Timeout = 300000;
        
        public const string GitShowCurrentBranch = "git branch --show-current";
        public const string GitTagList = "git tag";
        public const string GitCatObjectType = "git cat-file -t {0}";
        public const string GitGetCommitSha = "git rev-list -n 1 {0}";
        public const string GitApplyAnnotatedTag = "git tag -a {0} -m '{1}'";
        public const string GitLogWithShaAndBody = "git --no-pager log --pretty=format:\"%H %B<EOC>\"";
        public const string GitLogWithShaAndBodyInTopoOrder = "git --no-pager log --topo-order --pretty=format:\"%H %B<EOC>\"";
        public const string GitLogWithShaAndBodyInDateOrder = "git --no-pager log --date-order --pretty=format:\"%H %B<EOC>\"";
        public const string GitLogWithShaAndBodyInReverse = "git --no-pager log --reverse --pretty=format:\"%H %B<EOC>\"";
        
        public static string Run(string command) {
            using Process process = new();

#if OS_WINDOWS
            string programPath = null;
            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(d => d.IsReady))
                programPath = Directory.GetFiles(drive.RootDirectory.FullName, Program, SearchOption.AllDirectories)
                    .FirstOrDefault();
#else
            string programPath = Directory.GetFiles("/", Program, SearchOption.AllDirectories)
                .FirstOrDefault();
#endif

            if (programPath == null)
                throw new FileNotFoundException($"'{Program}' was not found on this system. Please install it and try again.");

            ProcessStartInfo startInfo = new($"{programPath} -c \"{command}\"") {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit(Timeout);

            if (process.ExitCode != 0)
                throw new OperationFailedException(
                    $"Shell command failed with error (exit code: {process.ExitCode})\n{process.StandardError.ReadToEnd()}");

            return process.StandardOutput.ReadToEnd().Trim();
        }
    }
}
