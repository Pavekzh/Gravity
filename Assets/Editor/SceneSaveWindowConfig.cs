using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CreateAssetMenu(fileName = "SceneSaveConfig", menuName = "ScriptableObjects/SceneSaveWindow")]
    public class SceneSaveWindowConfig : ScriptableObject
    {
        public Services.SaveSystemFactory SaveSystemFactory;

        public string StartSceneDirectory = "Assets/Resources/Presets/StartScene/";
        public string StartSceneName = "StartScene";

        public string PresetsDirectory = "Assets/Resources/Presets/Scenes/";
        public string PresetName = "New preset";
        public Services.FileNamesCollectionScriptableObject PresetFileNames;
    }
}