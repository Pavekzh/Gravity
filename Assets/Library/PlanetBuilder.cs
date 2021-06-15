using UnityEngine;

namespace Assets.Library
{
    class PlanetBuilder
    {
        public PlanetData Data { get; private set; }
        public PlanetBuilder(PlanetData data)
        {
            Data = data;
        }
        public GameObject CreatePlanet(Transform parent)
        {
            GameObject baseObject = Resources.Load<GameObject>("Prefabs/BasePlanet");
            if(baseObject == null)
            {
                ErrorManager.Instance.ShowErrorMessage("BasePlanet prefab was not found",this);
            }
            GameObject planetObject = GameObject.Instantiate(baseObject, parent);
            planetObject.name = Data.Name;
            Planet planet = planetObject.AddComponent<Planet>();

            foreach(ModuleData moduleData in Data.Modules)
            {
                Module module = planetObject.AddComponent(moduleData.ModuleMonoBeheviour) as Module;
                if(module == null)
                {
                    ErrorManager.Instance.ShowErrorMessage("Module must inherit Module type",this);
                }
                module.SetModule(moduleData);
                module.Planet = planet; 
            }
            return planetObject;
        }
    }
}
