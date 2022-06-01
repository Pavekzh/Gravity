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
        [SerializeField] private float frequency = 5;
        [SerializeField] private float lacunarity = 7.45f;
        [SerializeField] private float persistence = 0.2f;
        [SerializeField] private Vector3 centre = Vector3.zero;
        [SerializeField] private int octaves = 3;
        [SerializeField] private float minValue = 0.5f;
        [SerializeField] private float strength = 0.2f;

        public float Frequency { get => frequency; }
        public float Lacunarity { get => lacunarity; }
        public float Persistence { get => persistence; }
        public Vector3 Center { get=> centre; }
        public int Octaves { get => octaves; }
        public float MinValue { get => minValue; }
        public float Strength { get => strength; }

        public NoiseSettings() { }

        public NoiseSettings(NoiseSettings source)
        {
            this.frequency = source.frequency;
            this.lacunarity = source.lacunarity;
            this.persistence = source.persistence;
            this.centre = source.centre;
            this.octaves = source.octaves;
            this.minValue = source.minValue;
            this.strength = source.strength;
        }

        public NoiseSettings(float frequency,float lacunarity,float persistence,Vector3 center,int octaves,float minValue,float strength)
        {
            this.frequency = frequency;
            this.lacunarity = lacunarity;
            this.persistence = persistence;
            this.centre = center;
            this.octaves = octaves;
            this.minValue = minValue;
            this.strength = strength;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadToDescendant("Frequency");
            reader.Read();
            this.frequency = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.lacunarity = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.persistence = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            XmlSerializer vectorSerializer = new XmlSerializer(typeof(Vector3));
            this.centre = (Vector3)vectorSerializer.Deserialize(reader);

            reader.Read();
            this.octaves = int.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.minValue = float.Parse(reader.Value);
            reader.Read();
            reader.ReadEndElement();

            reader.Read();
            this.strength = float.Parse(reader.Value);
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

        public override bool Equals(object obj)
        {                
            NoiseSettings comparable = obj as NoiseSettings;
            if (comparable != null
                && comparable.Frequency == this.Frequency
                && comparable.Lacunarity == this.Lacunarity
                && comparable.Persistence == this.Persistence
                && comparable.Center == this.Center
                && comparable.Octaves == this.Octaves
                && comparable.MinValue == this.MinValue
                && comparable.Strength == this.Strength)
                return true;
            return false;
        }
    }
}
