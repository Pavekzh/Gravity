using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    [Serializable]
    public class SceneState:ICloneable
    {
        public string Name { get; set; }
        public List<PlanetData> Planets { get; set; } = new List<PlanetData>();


        public SceneState() { }
        public SceneState(List<PlanetData> planets)
        {
            Planets = planets;
        }

        public object Clone()
        {
            SceneState clonedState = this.MemberwiseClone() as SceneState;
            clonedState.Planets = new List<PlanetData>();
            foreach(PlanetData pData in this.Planets)
            {
                clonedState.Planets.Add(pData.Clone() as PlanetData);
            }
            return clonedState;
        }
    }
}
