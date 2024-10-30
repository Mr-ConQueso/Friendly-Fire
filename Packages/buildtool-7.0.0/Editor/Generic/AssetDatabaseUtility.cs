using System.IO;
using UnityEditor;

namespace SuperUnityBuild.BuildTool
{
    using UnityEngine;

    public static class AssetDatabaseUtility
    {
        public static void ImportAsset(string path, ImportAssetOptions options = ImportAssetOptions.Default)
        {
            #if UNITY_2021_2_OR_NEWER
                // Unity 2021.2+ fixes a bug in older Unity versions that required calling AssetDatabase.SaveAssets() before ImportAsset()
                // See <https://issuetracker.unity3d.com/issues/duplicating-asset-replaces-it-with-one-of-its-sub-assets-if-the-asset-is-created-in-a-version-before-fix>
                // TODO: Update this when fix is backported
            #else
                AssetDatabase.SaveAssets();
            #endif

            AssetDatabase.ImportAsset(path, options);
        }

        public static void EnsureDefaultDirectoriesExist()
        {
            string pluginRoot = Constants.PluginDirectory();
            string editorRoot = Path.Combine(pluginRoot, Constants.EditorDirectoryName);
            string buildActionsRoot = Path.Combine(editorRoot, Constants.BuildActionsDirectoryName);
            string settingsRoot = Path.Combine(pluginRoot, Constants.SettingsDirectoryName);

            CreateFolder(pluginRoot, Path.Combine(Constants.AssetsDirectoryName, Constants.PluginsDirectory), Constants.PluginDirectoryName);
            CreateFolder(editorRoot, pluginRoot, Constants.EditorDirectoryName);
            CreateFolder(buildActionsRoot, editorRoot, Constants.BuildActionsDirectoryName);
            CreateFolder(settingsRoot, pluginRoot, Constants.SettingsDirectoryName);
        }

        private static void CreateFolder(string path, string parentFolder, string folderName)
        {
            if (!Directory.Exists(path))
            {
                AssetDatabase.CreateFolder(parentFolder, folderName);
            }
        }
    }
}
