using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Builders;
using CommandDotNet.Execution;
using CommandDotNet.Rendering;

namespace Julesabr.GitBump.Middleware {
    public static class CustomVersionMiddleware {
        private static string VersionOptionName => Resources.A.Command_version;

        public static AppRunner UseCustomVersion(this AppRunner appRunner, AppVersionInfo info) {
            return appRunner.Configure(c => {
                c.UseMiddleware((context, next) => DisplayVersionIfSpecified(context, next, info),
                    MiddlewareSteps.Version);
                c.BuildEvents.OnCommandCreated += AddVersionOption;
            });
        }

        private static void AddVersionOption(BuildEvents.CommandCreatedEventArgs args) {
            if (!args.CommandBuilder.Command.IsRootCommand())
                return;

            if (args.CommandBuilder.Command.ContainsArgumentNode(Resources.A.Command_version))
                return;

            Option option = new(VersionOptionName, 'v',
                TypeInfo.Flag, ArgumentArity.Zero, BooleanMode.Implicit,
                typeof(CustomVersionMiddleware).FullName) {
                Description = Resources.A.Command_version_description,
                IsMiddlewareOption = true
            };

            args.CommandBuilder.AddArgument(option);
        }

        private static Task<int> DisplayVersionIfSpecified(
            CommandContext context,
            ExecutionDelegate next,
            AppVersionInfo info
        ) {
            if (!context.RootCommand!.HasInputValues(VersionOptionName))
                return next(context);

            Print(context.Console, info);
            return ExitCodes.Success;
        }

        private static void Print(IStandardOut console, AppVersionInfo info) {
            console.Out.WriteLine($"{info.FileName} {info.Version}");
        }
    }
}
