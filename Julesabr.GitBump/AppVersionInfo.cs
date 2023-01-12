using System.Reflection;
using CommandDotNet.Builders;

namespace Julesabr.GitBump {
    public sealed class AppVersionInfo {
        public string FileName { get; }
        public string Version { get; }

        public AppVersionInfo() {
            FileName = Assembly.GetEntryAssembly()?.GetName().Name!;
            Version = AppInfo.Instance.Version!;
        }

        public AppVersionInfo(string fileName, string version) {
            FileName = fileName;
            Version = version;
        }
    }
}
