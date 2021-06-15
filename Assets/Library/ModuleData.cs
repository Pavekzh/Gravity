using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml.Serialization;

namespace Assets.Library
{
    [Serializable]
    [XmlInclude(typeof(GravityModuleData))]
    public abstract class ModuleData
    {
        public abstract Type ModuleMonoBeheviour { get; }
    }
}
