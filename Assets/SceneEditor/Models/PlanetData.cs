using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using Assets.SceneEditor.Controllers;
using System.Xml.Schema;
using System.Xml;

namespace Assets.SceneEditor.Models
{
    [Serializable]
    public class PlanetData:IXmlSerializable,ICloneable
    {
        public PlanetData() { }

        public PlanetData(Dictionary<string, ModuleData> modules, string name,SceneObjectBuilder planetBuilder)
        {
            Modules = modules;
            Name = name;
            PlanetBuilder = planetBuilder;
        }

        [XmlIgnore]
        public Dictionary<string, ModuleData> Modules { get; private set; } = new Dictionary<string, ModuleData>();

        public Guid Guid { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public SceneObjectBuilder PlanetBuilder { get; set; }

        public T GetModule<T>(string key) where T : ModuleData
        {
            T result;
            ModuleData module;
            if(Modules.TryGetValue(key, out module))
            {
                result = module as T;
                if(result != null)
                {
                    return result;
                }
            }
            throw new Exception("Specified module doesn't exist or has wrong type");
        }

        public PlanetController CreateSceneObject()
        {
            GameObject planet = PlanetBuilder.Create(this);

            PlanetController controller = planet.AddComponent<PlanetController>();
            controller.PlanetData = this;
            foreach (KeyValuePair<string, ModuleData> valuePair in this.Modules)
            {
                controller.AddModule(valuePair.Value);
                valuePair.Value.CreateModule(planet);
            }

            Services.PlanetSelectSystem.Instance.PlanetControllers.Add(this.Guid, controller);
            Services.SceneStateManager.Instance.AddPlanet(this);
            return controller;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToAttribute("Name");
            this.Name = reader.Value;
            reader.MoveToElement();
            reader.ReadToDescendant("SceneObjectBuilder");
            XmlSerializer serializer = new XmlSerializer(typeof(SceneObjectBuilder));
            this.PlanetBuilder = serializer.Deserialize(reader) as SceneObjectBuilder;
            if (reader.ReadToDescendant("ModuleData"))
            {
                while (reader.LocalName == "ModuleData")
                {
                    XmlSerializer moduleSerializer = new XmlSerializer(typeof(ModuleData));
                    ModuleData data = (ModuleData)moduleSerializer.Deserialize(reader);
                    data.Planet = this;
                    data.OnDeserialized();
                    Modules.Add(data.Name, data);
                }
            }
            reader.Read();
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", this.Name);
            XmlSerializer serializer = new XmlSerializer(typeof(SceneObjectBuilder));
            serializer.Serialize(writer, this.PlanetBuilder);
            writer.WriteStartElement("Modules");
            foreach(KeyValuePair<string,ModuleData> mData in Modules)
            {
                XmlSerializer moduleSerializer = new XmlSerializer(typeof(ModuleData));
                moduleSerializer.Serialize(writer, mData.Value);
            }
            writer.WriteEndElement();
        }

        public object Clone()
        {
            PlanetData clonedData = this.MemberwiseClone() as PlanetData;
            clonedData.PlanetBuilder = this.PlanetBuilder.Clone() as SceneObjectBuilder;                
            clonedData.Modules = new Dictionary<string, ModuleData>();
            foreach(KeyValuePair<string,ModuleData> mData in Modules)
            {
                ModuleData clonedModule = mData.Value.Clone() as ModuleData;
                clonedModule.Planet = clonedData;
                clonedData.Modules.Add(mData.Key, clonedModule);

            }
            return clonedData;
        }
    }
}
