using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Generalisk.CleanBuild.Editor
{
    internal class CleanBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get; set; }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (!Debug.isDebugBuild) { return; } // Skip if Development Build

            EditorUtility.DisplayProgressBar("Hold on...", "Cleaning up...", 0);

            string path = new DirectoryInfo(report.summary.outputPath).Parent.FullName;
            
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(dir)) { continue; }
                if (dir.ToLower().Contains("donotship")
                    || dir.ToLower().Contains("dontship"))
                { Directory.Delete(dir, true); }
            }

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
