using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.SceneSimulation;

namespace Assets.SceneEditor.Models
{
    public class BodyCollisionModuleData : ModuleData
    {
        public override bool DisplayOnValuesPanel { get => false; }

        [System.Xml.Serialization.XmlIgnore]
        public override List<PropertyViewData> Properties => new List<PropertyViewData>();

        public override string Name => Key;

        public static string Key = "Collision";

        [System.Xml.Serialization.XmlIgnore]
        public override PlanetData Planet { get; set; }


        public override object Clone()
        {
            return new BodyCollisionModuleData();
        }

        public override void CreateModule(GameObject sceneObject)
        {
            sceneObject.AddComponent<BodyCollisionModule>();          
        }
    }
}
