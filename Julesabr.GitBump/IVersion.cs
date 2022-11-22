using System.Diagnostics.Contracts;

namespace Julesabr.GitBump {
    public interface IVersion {
        uint Major { get; }
        uint Minor { get; }
        uint Patch { get; }
        string PrereleaseBranch { get; }
        uint PrereleaseBuild { get; }
        bool IsPrerelease { get; }

        [Pure] IVersion BumpMajor();
        [Pure] IVersion BumpMinor();
        [Pure] IVersion BumpPatch();
        [Pure] IVersion BumpPrereleaseBuild();
    }
}