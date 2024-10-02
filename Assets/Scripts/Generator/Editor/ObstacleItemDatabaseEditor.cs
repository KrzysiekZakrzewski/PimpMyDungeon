using Item;
using UnityEditor;
using UnityEngine;

namespace Generator.Item
{
    [CustomEditor(typeof(ObstacleItemDatabase), true)]
    public class ObstacleItemDatabaseEditor : Editor
    {
        ObstacleItemDatabase database;

        private void Awake()
        {
            database = (ObstacleItemDatabase)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Sort Items"))
            {
                database.SortItem();
            }
        }
    }
}