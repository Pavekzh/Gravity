using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using BasicTools;
using System.IO;

namespace Assets.Services
{
    public class XMLSaveSystem : ISaveSystem
    {
        public string Extension => ".xml";

        public object Load(string fullPath, Type type)
        {
            try
            {
                if (fullPath != "")
                {
                    object state;
                    XmlSerializer serializer = new XmlSerializer(type);
                    using (FileStream file = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {

                        state = serializer.Deserialize(file);
                        return state;
                    }
                }

            }
            catch (System.Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }
            return null;
        }

        public void Save(object state, string fullPath)
        {
            try
            {
                if (fullPath != "")
                {
                    XmlSerializer serializer = new XmlSerializer(state.GetType());
                    File.Delete(fullPath);
                    using (FileStream file = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(file, state);
                    }
                }

            }
            catch (System.Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }
        }

        public void Delete(string fullPath)
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (System.Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }
        }

        public void Save(object state, Stream source)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(state.GetType());
                serializer.Serialize(source, state);
            }
            catch(Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }
        }

        public object Load(Stream source, Type type)
        {
            try
            {
                object state;
                XmlSerializer serializer = new XmlSerializer(type);
                state = serializer.Deserialize(source);
                return state;
            }
            catch(System.Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }
            return null;
        }
    }
}
