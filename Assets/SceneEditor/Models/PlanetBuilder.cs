using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Assets.SceneEditor.Models
{
    [XmlInclude(typeof(Assets.SceneSimulation.BasePlanetBuilder))]
    public abstract class PlanetBuilder:ICloneable
    {
        public abstract object Clone();

        public abstract GameObject Create(PlanetData data);
    }
}
