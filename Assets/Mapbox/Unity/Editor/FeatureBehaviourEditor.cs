namespace Mapbox.Editor
{
    using Mapbox.Unity.MeshGeneration.Components;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(FeatureBehaviour))]
    public class FeatureBehaviourEditor : Editor
    {
        FeatureBehaviour _beh;

        public void OnEnable()
        {
            _beh = (FeatureBehaviour)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Show Properties"))
            {
                _beh.ShowDebugData();
            }
        }
    }
}
