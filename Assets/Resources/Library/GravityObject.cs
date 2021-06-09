using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Resources.Library
{
    public abstract class GravityObject:MonoBehaviour
    {
        public abstract float Mass { get; }
        public abstract Vector3 Velocity { get; }
        public abstract Vector3 Position { get; }
    }
}
