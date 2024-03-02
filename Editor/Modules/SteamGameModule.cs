using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Nomnom.LCProjectPatcher.Editor.Modules {
    public static class SteamGameModule {
        private readonly static string[] DllsToCopy = new[] {
            "AIToASE.dll",
            "AmplifyImpostors.dll",
            "com.rlabrecque.steamworks.net.dll",
            "mcs.dll",
            "DemiLib.dll",
            "DOTween.dll",
            "DOTweenPro.dll",
            "FMODUnity.dll",
            "FMODUnityResonance.dll",
            "GlitchLibraryAssembly.dll",
            "jianglong.library.gif-player.dll",
            "PlasmaLibrary.dll",
            "QFSW.QC.dll",
            "QFSW.QC.Extras.dll",
            "QFSW.QC.Grammar.dll",
            "QFSW.QC.Parsers.dll",
            "QFSW.QC.ScanRules.dll",
            "QFSW.QC.Serializers.dll",
            "QFSW.QC.Suggestors.dll",
            "QFSW.QC.UI.dll",
            "Rewired_Core.dll",
            "Rewired_Windows.dll",
            "Sirenix.OdinInspector.Attributes.dll",
            "Sirenix.Serialization.Config.dll",
            "Sirenix.Serialization.dll",
            "Sirenix.Utilities.dll",
            "Assembly-CSharp-firstpass.dll",
        };
        
        private readonly static string[] SpecialDllsToCopy = new[] {
            "fmodstudio.dll",
            "PlasmaNativeCore.dll",
            "resonanceaudio.dll",
            "Rewired_DirectInput.dll",
            // "steam_api64.dll",
        };
        
        private readonly static string[] SpecialDllsToCopyIntoHidden = new[] {
            "steam_api64.dll",
        };
        
        public static void CopyManagedDlls(LCPatcherSettings settings) {
            var lcDataFolder = ModuleUtility.LethalCompanyDataFolder;
            var gameManagedFolder = Path.Combine(lcDataFolder, "Managed");
            var projectPluginsFolder = Path.Combine(settings.GetLethalCompanyGamePath(), "Plugins");
            
            Directory.CreateDirectory(projectPluginsFolder);
            
            for (var i = 0; i < DllsToCopy.Length; i++) {
                var dll = DllsToCopy[i];
                var gamePath = Path.Combine(gameManagedFolder, dll);
                var projectPath = Path.Combine(projectPluginsFolder, dll);

                EditorUtility.DisplayProgressBar("Copying game dlls", $"Copying {dll} to {projectPath}", (float)i / DllsToCopy.Length);

                if (!File.Exists(gamePath)) {
                    Debug.LogWarning($"Game dll \"{gamePath}\" does not exist");
                    continue;
                }

                try {
                    File.Copy(gamePath, projectPath, overwrite: true);
                } catch {
                    Debug.LogWarning($"Failed to copy {dll} to {projectPath}. The dll might be loaded, if so close unity to delete the file.");
                }
            }
            
            EditorUtility.ClearProgressBar();
        }

        public static void CopyPluginDlls(LCPatcherSettings settings) {
            var lcDataFolder = ModuleUtility.LethalCompanyDataFolder;
            var gameSpecialPluginsFolder = Path.Combine(lcDataFolder, "Plugins", "x86_64");
            
            var projectPluginsFolder = Path.Combine(settings.GetLethalCompanyGamePath(), "Plugins");
            var projectSpecialPluginsFolder = Path.Combine(projectPluginsFolder, "x86_64");
            var projectSpecialPluginsFolderHidden = Path.Combine(projectSpecialPluginsFolder, "Hidden~");
            
            Directory.CreateDirectory(projectSpecialPluginsFolder);
            Directory.CreateDirectory(projectSpecialPluginsFolderHidden);
            
            for (var i = 0; i < SpecialDllsToCopy.Length; i++) {
                var dll = SpecialDllsToCopy[i];
                var gamePath = Path.Combine(gameSpecialPluginsFolder, dll);
                var projectPath = Path.Combine(projectSpecialPluginsFolder, dll);

                EditorUtility.DisplayProgressBar("Copying game dlls", $"Copying {dll} to {projectPath}", (float)i / SpecialDllsToCopy.Length);

                if (!File.Exists(gamePath)) {
                    Debug.LogWarning($"Game dll \"{gamePath}\" does not exist");
                    continue;
                }

                try {
                    File.Copy(gamePath, projectPath, overwrite: true);
                } catch {
                    Debug.LogWarning($"Failed to copy {dll} to {projectPath}. The dll might be loaded, if so close unity to delete the file.");
                }
            }
            
            for (var i = 0; i < SpecialDllsToCopyIntoHidden.Length; i++) {
                var dll = SpecialDllsToCopyIntoHidden[i];
                var gamePath = Path.Combine(gameSpecialPluginsFolder, dll);
                var projectPath = Path.Combine(projectSpecialPluginsFolderHidden, dll);

                EditorUtility.DisplayProgressBar("Copying game dlls", $"Copying {dll} to {projectPath}", (float)i / SpecialDllsToCopyIntoHidden.Length);

                if (!File.Exists(gamePath)) {
                    Debug.LogWarning($"Game dll \"{gamePath}\" does not exist");
                    continue;
                }

                try {
                    File.Copy(gamePath, projectPath, overwrite: true);
                } catch {
                    Debug.LogWarning($"Failed to copy {dll} to {projectPath}. The dll might be loaded, if so close unity to delete the file.");
                }
            }
            
            EditorUtility.ClearProgressBar();
        }
    }
}
