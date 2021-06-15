using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library
{
    [Serializable]
    public class PlanetData
    {
        public PlanetData() { }
        public PlanetData(List<ModuleData> modules,string name)
        {
            Modules = modules;
            Name = name;
        }
        public List<ModuleData> Modules { get;  set; }
        public string Name { get;  set; }

    }
}
