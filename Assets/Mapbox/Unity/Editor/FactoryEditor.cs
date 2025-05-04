namespace Mapbox.Editor
{
    using Mapbox.Unity.MeshGeneration.Factories;
    using UnityEditor;

    [CustomEditor(typeof(AbstractTileFactory))]
    public class FactoryEditor : Editor
    {
        public override void OnInspectorGUI() { }
    }
}
