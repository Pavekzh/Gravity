using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library
{
    [Serializable]
    public class GravityModuleData:ModuleData
    {
        public override Type ModuleMonoBeheviour { get { return typeof(GravityModule); } }
        public float Mass { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public GravityModuleData() { }
        public GravityModuleData(float mass,Vector3 position,Vector3 velocity)
        {
            this.Mass = mass;
            this.Position = position;
            this.Velocity = velocity;
        }
    }
}
