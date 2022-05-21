using System;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace BasicTools
{
    [System.Serializable]
    public class NoiseSettings:IXmlSerializable
    {
        public float Frequency { get; private set; } = 5;
        public float Lacunarity { get; private set; } = 7.45f;
        public float Persistence { get; private set; } = 0.2f;
        public Vector3 Center { get; private set; } = Vector3.zero;
        public int Octaves { get; private set; } = 3;
        public float MinValue { get; private set; } = 0.5f;
        public float Strength { get; private set; } = 0.2f;

        public NoiseSettings() { }

        public NoiseSettings(float frequency,float lacunarity,float persistence,Vector3 center,int octaves,float minValue,float strength)
        {
            this.Frequency = frequency;
            this.Lacunarity = lacunarity;
            this.Persistence = persistence;
            this.Center = center;
            this.Octaves = octaves;
            this.MinValue = minValue;
            this.Strength = strength;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadToDescendant("Frequency");
            reader.Read();
            this.Frequency = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.Lacunarity = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.Persistence = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            XmlSerializer vectorSerializer = new XmlSerializer(typeof(Vector3));
            this.Center = (Vector3)vectorSerializer.Deserialize(reader);

            reader.Read();
            this.Octaves = int.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.MinValue = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.Strength = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Frequency", this.Frequency.ToString());
            writer.WriteElementString("Lacunarity", this.Lacunarity.ToString());
            writer.WriteElementString("Persistence", this.Persistence.ToString());
            XmlSerializer vectorSerializer = new XmlSerializer(typeof(Vector3));
            vectorSerializer.Serialize(writer, this.Center);

            writer.WriteElementString("Octaves", this.Octaves.ToString());
            writer.WriteElementString("MinValue", this.MinValue.ToString());
            writer.WriteElementString("Strength", this.Strength.ToString());
        }
    }
}
