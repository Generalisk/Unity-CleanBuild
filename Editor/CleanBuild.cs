using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Generalisk.CleanBuild.Editor
{
    internal class CleanBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get; set; }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Skip if Development Build
            if (EditorUserBuildSettings.development) { return; }

            // Remove Do not Ship folders
            EditorUtility.DisplayProgressBar("Cleaning up", "\"Do not Ship\" folders", 0);

            string path = new DirectoryInfo(report.summary.outputPath).Parent.FullName;
            
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(dir)) { continue; }
                if (dir.ToLower().Contains("donotship")
                    || dir.ToLower().Contains("dontship"))
                { Directory.Delete(dir, true); }
            }

            // Remove Debug files (.pdb)
            EditorUtility.DisplayProgressBar("Cleaning up", ".pdb files", 0.5f);

            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.ToLower().EndsWith(".pdb"))
                { File.Delete(file); }
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
