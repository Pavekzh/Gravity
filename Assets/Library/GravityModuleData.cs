using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library
{
    [Serializable]
    class GravityModuleData:ModuleData
    {
        public override Type ModuleMonoBeheviour { get { return typeof(GravityModule); } }
        public float Mass { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }

        public GravityModuleData(float mass,Vector3 position,Vector3 velocity)
        {
            this.Mass = mass;
            this.Position = position;
            this.Velocity = velocity;
        }
    }
}
