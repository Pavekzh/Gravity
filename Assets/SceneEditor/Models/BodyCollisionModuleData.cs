using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.SceneSimulation;
using Assets.Services;

namespace Assets.SceneEditor.Models
{
    public class BodyCollisionModuleData : ModuleData
    {

        public override bool DisplayOnValuesPanel { get => false; }
        public override List<PropertyViewData> Properties => new List<PropertyViewData>();
        public override Type ModuleType => typeof(BodyCollisionModule);
        public override string Name => Key;

        public static string Key = "Collision";

        [System.Xml.Serialization.XmlIgnore]
        public override PlanetData Planet { get; set; }

        public override object Clone()
        {
            return new BodyCollisionModuleData();
        }

    }
}
