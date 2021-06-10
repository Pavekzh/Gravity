using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library
{
    public interface IGravityObject
    {
        float Mass { get;}
        Vector3 Position { get; }
        Vector3 Velocity { get; }
    }
}
