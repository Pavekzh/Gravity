using Assets.SceneEditor.Models;
using System.Collections.Generic;
using Assets.Services;
using UnityEngine;
using Assets.SceneEditor.Controllers;


namespace Assets.SceneSimulation
{
    [System.Serializable]
    public class BasePlanetBuilder : PlanetBuilder
    {
        public BasePlanetBuilder() { }

        public override GameObject Create(PlanetData data)
        {
            if (data != null)
            {
                if (PlanetBuildSettings.Instance.BasePlanetPrefab == null)
                {
                    CommonErrorManager.Instance.ShowErrorMessage("BasePlanet prefab was not set", this);
                }
                GameObject planetObject = GameObject.Instantiate(PlanetBuildSettings.Instance.BasePlanetPrefab, PlanetBuildSettings.Instance.PlanetsParent);
                planetObject.name = data.Name;

                return planetObject;
            }
            else
                throw new System.Exception("BasePlanetBuilder: PlanetData was not set");
        }

        public override object Clone()
        {
            return new BasePlanetBuilder();
        }
    }
}
