using CommandLine;
using JetBrains.Annotations;
using Julesabr.IO;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    [UsedImplicitly]
    internal static class Program {
        [UsedImplicitly]
        public static int Main(string[] args) {
            Controller controller = new(IRepository.Create(), new FileFactory());
            Options options = Parser.Default.ParseArguments<Options>(args).Value;
            
            return (int) controller.GitBump(options);
        }
    }
}
