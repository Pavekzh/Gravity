using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Services;
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
        public virtual int DisplayIndex { get => -1; }

        protected bool isDeserialized = false;

        [XmlIgnore]
        public abstract List<PropertyViewData> Properties { get; }
        public abstract System.Type ModuleType { get; }
        public abstract string Name { get; }
        [XmlIgnore]
        public abstract PlanetData Planet { get; set; }

        public abstract object Clone();

        public virtual void OnDeserialized()
        {
            isDeserialized = true;
        }
    }
}
