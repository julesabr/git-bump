using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using JetBrains.Annotations;
using Julesabr.IO;
using Julesabr.LibGit;

namespace Julesabr.GitBump {
    [UsedImplicitly]
    internal static class Program {
        private const string Name = "git bump";
        
        [UsedImplicitly]
        public static int Main(string[] args) {
            int exitCode = 0;

            Parser parser = new(with => {
                with.AutoHelp = false;
                with.AutoVersion = false;
                with.HelpWriter = null;
            });

            ParserResult<Options> result = parser.ParseArguments<Options>(args);
            result.WithParsed(o => {
                if (o.Version) {
                    Console.WriteLine(
                        $"{Name} {Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.1.0"}");
                    return;
                }
                
                if (o.Help) {
                    DisplayHelp(result, Console.Out);
                    return;
                }
                
                Controller controller = new(IRepository.Create(), new FileFactory());
                exitCode = (int)controller.GitBump(o);
            }).WithNotParsed(e => {
                Console.Error.WriteLine($"git-bump: error: {e.First()}");
                DisplayHelp(result, Console.Error);
                exitCode = 1;
            });

            return exitCode;
        }

        private static void DisplayHelp(ParserResult<Options> result, TextWriter writer) {
            HelpText helpText = HelpText.AutoBuild(result, h => {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = $"Usage: {Name} [-c|--channel \"<channel>\"] [-p|--prefix \"<prefix>\"]\n" +
                            "                [-s|--suffix \"<suffix>\"] [--version-output \"<version-output>\"]\n" +
                            "                [--prerelease] [--push] [-t|--tag] [-h|--help] [-v|--version]\n" +
                            "\n" +
                            "Show, create, or push the bumped version.";
                h.Copyright = "";

                return h;
            });
            writer.WriteLine(helpText);
        }
    }
}
