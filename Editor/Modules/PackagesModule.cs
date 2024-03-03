using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Nomnom.LCProjectPatcher.Editor.Modules {
    public static class PackagesModule {
        public readonly static (string, string)[] Packages = new[] {
            ("com.unity.2d.sprite", "1.0.0"),
            ("com.unity.2d.tilemap", "1.0.0"),
            ("com.unity.ai.navigation", "1.1.3"),
            ("com.unity.services.analytics", "4.4.0"),
            ("com.unity.analytics", "3.8.1"),
            //("com.unity.nuget.newtonsoft-json", "3.2.1"),
            ("com.unity.polybrush", "1.1.4"),
            ("com.unity.postprocessing", "3.2.2"),
            ("com.unity.services.core", "1.9.0"),
            ("com.unity.formats.usd","3.0.0-exp.1"),
        };

        public readonly static string[] GitPackages = new[] {
            "https://github.com/Unity-Technologies/AssetBundles-Browser.git",
        };
        
        public static bool InstallAll() {
            ImportTMP();
            
            var packageStrings = Packages
                .Select(x => x.Item2 == null ? x.Item1 : $"{x.Item1}@{x.Item2}")
                .ToArray();
            var allPackageStrings = packageStrings
                .Concat(GitPackages)
                .ToArray();
            
            // check if packages are already installed
            EditorUtility.DisplayProgressBar("Installing packages", "Checking if packages are already installed", 0.25f);
            try {
                Debug.Log("Checking if packages are already installed");
                var installedPackages = Client.List(false, false);
                while (!installedPackages.IsCompleted) {
                    // await UniTask.Delay(1, ignoreTimeScale: true);
                }
                
                var allAreInstalled = packageStrings.All(x => installedPackages.Result.Count(y => y.packageId == x) > 0);
                if (allAreInstalled) {
                    EditorUtility.ClearProgressBar();
                    Debug.Log("Packages already installed");
                    return false;
                }
            
                EditorUtility.DisplayProgressBar("Installing packages", $"Installing {allPackageStrings.Length} package{(allPackageStrings.Length == 1 ? string.Empty : "s")}", 0.5f);
                var request = Client.AddAndRemove(allPackageStrings);
                while (!request.IsCompleted) {
                    // await UniTask.Delay(1, ignoreTimeScale: true);
                }
            
                Client.Resolve();
                EditorUtility.ClearProgressBar();
            } catch(Exception e) {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Failed to get installed packages: {e}");
                return false;
            }
            
            Debug.Log("Packages installed");
            return true;
        }

        private static void ImportTMP() {
            // import the TMP package automatically
            EditorUtility.DisplayProgressBar("Installing packages", "Installing TMP Essential Resources", 1f);
            AssetDatabase.ImportPackage("Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage", false);
            EditorUtility.ClearProgressBar();
        }
    }
}
