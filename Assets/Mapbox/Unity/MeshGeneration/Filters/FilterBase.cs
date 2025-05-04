namespace Mapbox.Unity.MeshGeneration.Filters
{
    using System;
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public interface ILayerFeatureFilterComparer
    {
        bool Try(VectorFeatureUnity feature);
    }

    public class FilterBase : ILayerFeatureFilterComparer
    {
        public virtual string Key
        {
            get { return ""; }
        }

        public virtual bool Try(VectorFeatureUnity feature)
        {
            return true;
        }

        public virtual void Initialize() { }
    }
}
