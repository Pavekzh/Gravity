using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library
{  
    [Serializable]
    public class SceneState
    {
        public List<PlanetData> PlanetsData { get;  set; }
        public SceneState() { }
        public SceneState(List<PlanetData> planets)
        {
            PlanetsData = planets;
        }
    }
}
