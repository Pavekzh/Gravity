using Assets.SceneEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Services
{
    public class MassEstimator : IPlanetEstimator<float>
    {
        public float Estimate(PlanetData planet)
        {
            return planet.GetModule<GravityModuleData>(GravityModuleData.Key).Mass;
        }
    }
}
