using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommandDotNet;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Controllers {
    public static class ErrorHandler {
        public static int Invoke(CommandContext? context, Exception cause) {
            if (context == null) {
                Console.Error.WriteLine("git-bump: error: Command context is missing");
                return (int)ExitCode.Max;
            }

            switch (cause) {
                case ValidationException:
                    context.Console.Error.WriteLine($"git-bump: validation error: {cause.Message}");
                    PrintHelp(context, context.Console.Error);
                    return (int)ExitCode.ValidationError;
                case ArgumentException:
                    context.Console.Error.WriteLine($"git-bump: illegal state: {cause.Message}");
                    return (int)ExitCode.IllegalState;
                case FileNotFoundException:
                    context.Console.Error.WriteLine($"git-bump: file not found: {cause.Message}");
                    return (int)ExitCode.FileNotFound;
                case OperationFailedException:
                    context.Console.Error.WriteLine($"git-bump: operation failed: {cause.Message}");
                    return (int)ExitCode.OperationFailed;
                default:
                    context.Console.Error.WriteLine($"git-bump: error: {cause.Message}");
                    return (int)ExitCode.Fail;
            }
        }

        private static void PrintHelp(CommandContext commandContext, TextWriter writer) {
            string helpText = commandContext.AppConfig.HelpProvider.GetHelpText(commandContext.RootCommand!);
            writer.WriteLine(helpText);
        }
    }
}
