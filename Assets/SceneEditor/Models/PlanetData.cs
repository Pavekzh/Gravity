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

        public PlanetData(Dictionary<string, ModuleData> modules, string name)
        {
            Modules = modules;
            Name = name;
        }

        [XmlIgnore]
        public Dictionary<string, ModuleData> Modules { get; private set; } = new Dictionary<string, ModuleData>();

        public Guid Guid { get; } = Guid.NewGuid();
        public string Name { get; set; }

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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToAttribute("Name");
            this.Name = reader.Value;
            reader.MoveToElement();
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
            PlanetData clonedData = new PlanetData();
            clonedData.Name = this.Name;            
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
