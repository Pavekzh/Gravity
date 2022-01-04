using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Assets.SceneEditor.Models
{
    [XmlInclude(typeof(Assets.SceneSimulation.DefaultSceneObjectBuilder))]
    public abstract class SceneObjectBuilder:ICloneable
    {
        public abstract object Clone();

        public abstract GameObject Create(PlanetData data);
    }
}
