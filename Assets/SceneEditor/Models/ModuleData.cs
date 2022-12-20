using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.SceneSimulation;
using System.Xml.Schema;
using System.Xml;

namespace Assets.SceneEditor.Models
{
    [XmlInclude(typeof(GravityModuleData))]
    [XmlInclude(typeof(PlanetViewModuleData))]
    [XmlInclude(typeof(BodyCollisionModuleData))]
    public abstract class ModuleData:System.ICloneable
    {
        public virtual bool DisplayOnValuesPanel { get => true; }

        [XmlIgnore]
        public abstract List<PropertyViewData> Properties { get; }
        public abstract string Name { get; }
        [XmlIgnore]
        public abstract PlanetData Planet { get; set; }

        public abstract object Clone();

        public abstract void CreateModule(GameObject sceneObject);
        public abstract void OnDeserialized();
    }
}
