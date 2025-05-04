namespace Mapbox.Unity.Map
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Modifiers;
    using UnityEngine;

    [Serializable]
    public class SpawnPrefabOptions : ModifierProperties
    {
        public override Type ModifierType
        {
            get { return typeof(PrefabModifier); }
        }

        public GameObject prefab;
        public bool scaleDownWithWorld = true;

        [NonSerialized]
        public Action<List<GameObject>> AllPrefabsInstatiated = delegate { };
    }
}
