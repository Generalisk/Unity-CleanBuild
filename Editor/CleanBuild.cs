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
                        new KeyValuePair<string, string>("com.unity.modules.accessibility", "UnityEngine.AccessibilityModule"),
                        new KeyValuePair<string, string>("com.unity.modules.ai", "UnityEngine.AIModule"),
                        new KeyValuePair<string, string>("com.unity.modules.amd", "UnityEngine.AMDModule"),
                        new KeyValuePair<string, string>("com.unity.modules.androidjni", "UnityEngine.AndroidJNIModule"),
                        new KeyValuePair<string, string>("com.unity.modules.animation", "UnityEngine.AnimationModule"),
                        new KeyValuePair<string, string>("com.unity.modules.assetbundle", "UnityEngine.AssetBundleModule"),
                        new KeyValuePair<string, string>("com.unity.modules.audio", "UnityEngine.AudioModule"),
                        new KeyValuePair<string, string>("com.unity.modules.cloth", "UnityEngine.ClothModule"),
                        new KeyValuePair<string, string>("com.unity.modules.director", "UnityEngine.DirectorModule"),
                        new KeyValuePair<string, string>("com.unity.modules.hierarchycore", "UnityEngine.HierarchyCoreModule"),
                        new KeyValuePair<string, string>("com.unity.modules.imageconversion", "UnityEngine.ImageConversionModule"),
                        new KeyValuePair<string, string>("com.unity.modules.imgui", "UnityEngine.IMGUIModule"),
                        new KeyValuePair<string, string>("com.unity.modules.jsonserialize", "UnityEngine.JSONSerializeModule"),
                        new KeyValuePair<string, string>("com.unity.modules.nvidia", "UnityEngine.NVIDIAModule"),
                        new KeyValuePair<string, string>("com.unity.modules.particlesystem", "UnityEngine.ParticleSystemModule"),
                        new KeyValuePair<string, string>("com.unity.modules.physics", "UnityEngine.PhysicsModule"),
                        new KeyValuePair<string, string>("com.unity.modules.physics2d", "UnityEngine.Physics2DModule"),
                        new KeyValuePair<string, string>("com.unity.modules.screencapture", "UnityEngine.ScreenCaptureModule"),
                        new KeyValuePair<string, string>("com.unity.modules.subsystems", "UnityEngine.SubsystemsModule"),
                        new KeyValuePair<string, string>("com.unity.modules.terrain", "UnityEngine.TerrainModule"),
                        new KeyValuePair<string, string>("com.unity.modules.terrainphysics", "UnityEngine.TerrainPhysicsModule"),
                        new KeyValuePair<string, string>("com.unity.modules.tilemap", "UnityEngine.TilemapModule"),
                        new KeyValuePair<string, string>("com.unity.modules.ui", "UnityEngine.UIModule"),
                        new KeyValuePair<string, string>("com.unity.modules.uielements", "UnityEngine.UIElementsModule"),
                        new KeyValuePair<string, string>("com.unity.modules.umbra", "UnityEngine.UmbraModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unityanalytics", "UnityEngine.UnityAnalyticsModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unitywebrequestassetbundle", "UnityEngine.UnityWebRequestAssetBundleModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unitywebrequestaudio", "UnityEngine.UnityWebRequestAudioModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unitywebrequest", "UnityEngine.UnityWebRequestModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unitywebrequesttexture", "UnityEngine.UnityWebRequestTextureModule"),
                        new KeyValuePair<string, string>("com.unity.modules.unitywebrequestwww", "UnityEngine.UnityWebRequestWWWModule"),
                        new KeyValuePair<string, string>("com.unity.modules.vehicles", "UnityEngine.VehiclesModule"),
                        new KeyValuePair<string, string>("com.unity.modules.video", "UnityEngine.VideoModule"),
                        new KeyValuePair<string, string>("com.unity.modules.vr", "UnityEngine.VRModule"),
                        new KeyValuePair<string, string>("com.unity.modules.wind", "UnityEngine.WindModule"),
                        new KeyValuePair<string, string>("com.unity.modules.xr", "UnityEngine.XRModule"),
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
