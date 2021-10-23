using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.SceneSimulation;
using System.Xml.Schema;
using System.Xml;

namespace Assets.SceneEditor.Models
{
    [XmlInclude(typeof(GravityModuleData))]
    public abstract class ModuleData:System.ICloneable
    {
        [XmlIgnore]
        public abstract List<PropertyViewData> Properties { get; }
        public abstract string Name { get; }

        public abstract object Clone();

        public abstract void CreateModule(GameObject sceneObject);
    }
}
