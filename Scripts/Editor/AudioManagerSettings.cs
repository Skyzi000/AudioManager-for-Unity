using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Skyzi000.AudioManager.Editor
{
    public class AudioManagerSettings : ScriptableObject
    {
        private static readonly string DirectoryPath = Path.Combine("Assets", "AudioManagerGenerated");

        private static readonly string AudioManagerPrefabName = nameof(AudioManager);

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var prefab = Resources.Load<GameObject>(AudioManagerPrefabName);
            if (prefab == null)
                GenerateAssets();
        }

        [MenuItem("Assets/AudioManager/Generate assets")]
        private static void GenerateAssets()
        {
            ScriptableObject newSettings = CreateInstance(typeof(AudioManagerSettings));
            var thisScriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(newSettings));
            DirectoryInfo parentDirectory = Directory.GetParent(thisScriptPath)?.Parent?.Parent;
            if (!(parentDirectory is { Exists: true }))
                throw new InvalidOperationException($"Directory not found: {parentDirectory}");
            Directory.CreateDirectory(DirectoryPath);
            GameObject managerPrefab = CopyManagerPrefab(parentDirectory.FullName);
            AudioMixer mixer = CopyAudioMixer(parentDirectory.FullName);
            GameObject managerTempInstance = Instantiate(managerPrefab);
            var am = managerTempInstance.GetComponentInChildren<AudioManager>();
            am.Mixer = mixer;
            am.BGMGroup = mixer.FindMatchingGroups("BGM")[0];
            am.SEGroup = mixer.FindMatchingGroups("SE")[0];
            managerPrefab = PrefabUtility.SaveAsPrefabAsset(managerTempInstance,
                Path.Combine(DirectoryPath, "Resources", $"{AudioManagerPrefabName}.prefab"));
            AssetDatabase.SaveAssetIfDirty(managerPrefab);
            DestroyImmediate(managerTempInstance);
        }

        private static GameObject CopyManagerPrefab(string parentDirectoryPath)
        {
            var srcPath = Path.Combine(parentDirectoryPath, "Prefabs", $"{AudioManagerPrefabName}.prefab");
            var destPath = Path.Combine(DirectoryPath, "Resources", $"{AudioManagerPrefabName}.prefab");
            Directory.CreateDirectory(Path.Combine(DirectoryPath, "Resources"));
            File.Copy(srcPath, destPath, false);
            AssetDatabase.ImportAsset(destPath);
            return AssetDatabase.LoadAssetAtPath<GameObject>(destPath);
        }

        private static AudioMixer CopyAudioMixer(string parentDirectoryPath)
        {
            var srcPath = Path.Combine(parentDirectoryPath, "Mixer", "AudioMixer.mixer");
            var destPath = Path.Combine(DirectoryPath, "Mixer", "AudioMixer.mixer");
            Directory.CreateDirectory(Path.Combine(DirectoryPath, "Mixer"));
            File.Copy(srcPath, destPath, false);
            AssetDatabase.ImportAsset(destPath);
            return AssetDatabase.LoadAssetAtPath<AudioMixer>(destPath);
        }
    }
}
