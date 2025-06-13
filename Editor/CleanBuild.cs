using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;

namespace Generalisk.CleanBuild.Editor
{
    internal class CleanBuild : IPostprocessBuildWithReport
    {
        // Make sure the clean build is ran last in the build
        public int callbackOrder { get; set; } = int.MaxValue;

        public void OnPostprocessBuild(BuildReport report)
        {
            // Skip if Development Build
            if (EditorUserBuildSettings.development) { return; }

            // Setup Variables
            string path = new DirectoryInfo(report.summary.outputPath).FullName;
            if (File.Exists(path)) { path = new DirectoryInfo(path).Parent.FullName; }

            BuildTarget target = report.summary.platform;
            BuildTargetGroup targetGroup = report.summary.platformGroup;
            // Old method is deprecated since Unity 2021.2, replace with new method
#if NEW_SCRIPTING_BACKEND_GET_IMPLIMENTATION
            NamedBuildTarget namedTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(namedTarget);
#else // NEW_SCRIPTING_BACKEND_GET_IMPLIMENTATION
            ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
#endif // NEW_SCRIPTING_BACKEND_GET_IMPLIMENTATION

            // Remove Do not Ship folders
            DisplayProgressBar("\"Do not Ship\" folders", 0);
            
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(dir)) { continue; }
                if (dir.ToLower().Contains("donotship")
                    || dir.ToLower().Contains("dontship"))
                { Directory.Delete(dir, true); }
            }

            // Remove Debug files (.pdb)
            DisplayProgressBar(".pdb files", 0.25f);

            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.ToLower().EndsWith(".pdb"))
                { File.Delete(file); }
            }

            // Remove D3D12 Folder (if graphics api is not included in project)
            DisplayProgressBar("Direct3D12 Binaries", 0.5f);

            if (targetGroup == BuildTargetGroup.Standalone)
            {
                if (!PlayerSettings.GetGraphicsAPIs(target).Contains(GraphicsDeviceType.Direct3D12))
                {
                    if (Directory.Exists(path + "/D3D12"))
                    { Directory.Delete(path + "/D3D12", true); }
                }
            }

            // Remove Unused built-in modules (mono standalone only)
            DisplayProgressBar("Unused built-in modules", 0.75f);

            if (targetGroup == BuildTargetGroup.Standalone)
            {
                if (scriptingBackend == ScriptingImplementation.Mono2x)
                {
                    ListRequest request = Client.List(true, true);
                    while (!request.IsCompleted) { }
                    PackageCollection packages = request.Result;

                    KeyValuePair<string, string>[] modules = {
                        // Package ID, DLL Name
                        new KeyValuePair<string, string>("com.unity.modules.vr", "UnityEngine.VRModule"),
                        new KeyValuePair<string, string>("com.unity.modules.xr", "UnityEngine.ARModule"),
                        new KeyValuePair<string, string>("com.unity.modules.xr", "UnityEngine.XRModule"),
                        // TODO: Finish implimentation of this list and make sure that the deleted DLL will not cause A crash
                    };

                    string managedDirectory = Application.productName + "_Data/Managed";
                    if (target == BuildTarget.StandaloneOSX)
                    { managedDirectory = "Contents/Resources/Data/Managed"; }

                    foreach (KeyValuePair<string, string> module in modules)
                    {
                        bool exists = packages.Where(x => x.name == module.Key).ToArray().Length > 0;

                        if (!exists)
                        {
                            string dllPath = path + "/" + managedDirectory + "/" + module.Value + ".dll";

                            if (File.Exists(dllPath)) { File.Delete(dllPath); }
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }

        private void DisplayProgressBar(string text, float progress)
        { EditorUtility.DisplayProgressBar("Cleaning up", text, progress); }
    }
}
