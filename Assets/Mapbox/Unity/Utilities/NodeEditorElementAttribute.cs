namespace Mapbox.Unity.Utilities
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class NodeEditorElementAttribute : Attribute
    {
        public string Name;

        public NodeEditorElementAttribute(string s)
        {
            Name = s;
        }
    }
}
