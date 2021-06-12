using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library
{  
    public class SceneState
    {
        public List<PlanetData> PlanetsData { get; private set; }
        public SceneState(List<PlanetData> planets)
        {
            PlanetsData = planets;
        }
    }
}
