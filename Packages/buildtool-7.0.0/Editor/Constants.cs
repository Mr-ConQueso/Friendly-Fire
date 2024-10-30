namespace SuperUnityBuild.BuildTool
{
    using System.IO;

    public static class Constants
    {
        public const string PluginsDirectory = "Editor";
        public const string AssetsDirectoryName = "Assets";

        public static string PluginDirectory() { return Path.Combine(AssetsDirectoryName, PluginsDirectory, PluginDirectoryName); }
        public const string PluginDirectoryName = "SuperUnityBuild";

        public const string BuildActionsDirectoryName = "BuildActions";
        public const string DefaultSettingsFileName = "SuperUnityBuildSettings";
        public const string EditorDirectoryName = "Editor";
        public const string SettingsDirectoryName = "Settings";
    }
}
