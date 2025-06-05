using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.Rendering;

namespace Generalisk.CleanBuild.Editor
{
    internal class CleanBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get; set; }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Skip if Development Build
            if (EditorUserBuildSettings.development) { return; }

            // Setup Variables
            string path = new DirectoryInfo(report.summary.outputPath).Parent.FullName;
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

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
            DisplayProgressBar(".pdb files", 0.334f);

            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.ToLower().EndsWith(".pdb"))
                { File.Delete(file); }
            }

            // Remove D3D12 Folder (if graphics api is not included in project)
            DisplayProgressBar("Direct3D12 Binaries", 0.667f);

            if (!PlayerSettings.GetGraphicsAPIs(target).Contains(GraphicsDeviceType.Direct3D12))
            {
                if (Directory.Exists(path + "/D3D12"))
                { Directory.Delete(path + "/D3D12", true); }
            }

            EditorUtility.ClearProgressBar();
        }

        private void DisplayProgressBar(string text, float progress)
        { EditorUtility.DisplayProgressBar("Cleaning up", text, progress); }
    }
}
