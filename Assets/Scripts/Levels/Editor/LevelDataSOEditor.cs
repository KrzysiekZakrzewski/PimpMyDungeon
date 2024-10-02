using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Levels.Data.Editors
{
    [CustomEditor(typeof(LevelDataSO))]
    public class LevelDataSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (LevelDataSO)target;

            script.IsOverride = EditorGUILayout.Toggle("PlaceablePositionOverride", script.IsOverride);

            if (!script.IsOverride)
                return;

            if(GUILayout.Button("Generate override Data"))
            {
                script.GeneratePlaceableItemData();
            }
        }
    }
}