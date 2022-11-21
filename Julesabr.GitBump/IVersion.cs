namespace Julesabr.GitBump {
    public interface IVersion {
        uint Major { get; }
        uint Minor { get; }
        uint Patch { get; }
        string PrereleaseBranch { get; }
        uint PrereleaseBuild { get; }
        bool IsPrerelease { get; }

        Version BumpMajor();
        IVersion BumpMinor();
        IVersion BumpPatch();
    }
}