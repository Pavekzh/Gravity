using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library
{
    [Serializable]
    public abstract class ModuleData
    {
        public abstract Type ModuleMonoBeheviour { get; }
    }
}
