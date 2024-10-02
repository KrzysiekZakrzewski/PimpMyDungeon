using Saves.Object;
using UnityEditor;
using UnityEngine;

namespace Saves.Editors
{
    [CustomEditor(typeof(LevelSaveObject))]
    public class LevelObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelSaveObject levelSaveObject = (LevelSaveObject)target;

            if(GUILayout.Button("Clear Saves"))
            {
                levelSaveObject.ClearSaves();
            }
            if (GUILayout.Button("Unlock All"))
            {
                levelSaveObject.UnlockAll();
            }
        }
    }
}