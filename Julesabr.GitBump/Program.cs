using CommandDotNet;
using CommandDotNet.Builders;
using CommandDotNet.Help;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using Julesabr.IO;
using Julesabr.LibGit;
using Microsoft.Extensions.DependencyInjection;

namespace Julesabr.GitBump {
    public static class Program {
        private const string Name = "git bump";

        public static int Main(string[] args) {
            ServiceCollection services = new();

            services.AddSingleton<Controller>();
            services.AddSingleton(IRepository.Create());
            services.AddSingleton<FileFactory>();

            MicrosoftDependencyInjectionResolver resolver = new(services.BuildServiceProvider());

            return GetAppRunner(resolver)
                .UseCustomVersion(new AppVersionInfo())
                .Run(args);
        }

        public static AppRunner GetAppRunner(IDependencyResolver dependencyResolver) {
            AppSettings settings = new() {
                Help = new AppHelpSettings {
                    PrintHelpOption = true,
                    ExpandArgumentsInUsage = true,
                    TextStyle = HelpTextStyle.Detailed,
                    UsageAppName = Name
                }
            };

            return new AppRunner<Controller>(settings)
                .UseDependencyResolver(dependencyResolver)
                .UseErrorHandler(ErrorHandler.Invoke);
        }
    }
}
