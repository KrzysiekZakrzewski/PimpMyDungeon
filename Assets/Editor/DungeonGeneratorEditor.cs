using Item;
using Levels.Data;
using UnityEditor;
using UnityEngine;

namespace Generator
{
    public class DungeonGeneratorEditor : EditorWindow
    {
        [SerializeField]
        private ObstacleItemDatabase ObstacleItemDatabase; 
        private static readonly string DestinationPath = "Assets/Data/Levels/";
        private string textField;

        private static RandomDungeonGenerator dungeonGenerator;
        private LevelDataSO levelDataObject;

        [MenuItem("BlueRacon/DungeonGenerator")]
        private static void Init()
        {
            EditorWindow window = GetWindow(typeof(DungeonGeneratorEditor));
            window.Show();
            dungeonGenerator = FindObjectOfType<RandomDungeonGenerator>();
        }

        private void OnGUI()
        {
            textField = EditorGUILayout.TextField("Dungeon name: ", textField);

            if (GUILayout.Button("Generate Dungeon"))
            {
                GenerateDungeon();
            }

            if(GUILayout.Button("Save Dungeon"))
            {
                SaveDungeonToAsset();
            }
        }

        private void GenerateDungeon()
        {
            levelDataObject = dungeonGenerator.GenerateDungeon();
        }

        private void SaveDungeonToAsset()
        {
            if (levelDataObject == null)
            {
                Debug.LogError("Level Object is null. First generate dungeon");
                return;
            }

            if (textField == null || string.IsNullOrEmpty(textField) || string.IsNullOrWhiteSpace(textField))
                return;

            var path = $"{DestinationPath}{textField}.asset";

            levelDataObject.SetupName(textField);

            AssetDatabase.CreateAsset(levelDataObject, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Level Data saved succesfull in: {path}");
        }
    }
}