using UnityEditor;
using UnityEngine;

namespace Marketing.Editors
{
    [CustomEditor(typeof(TakeScreenCapture))]
    public class TakeScreenCaptureEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (TakeScreenCapture)target;

            if (GUILayout.Button("TakeScreen"))
            {
                script.TakeScreenShoot();
            }
        }
    }
}