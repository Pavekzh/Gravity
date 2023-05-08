using Assets.SceneEditor.Models;
using Assets.SceneEditor.Controllers;
using System.Linq;
using System;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace Assets.Services
{
    public class CommonPlanetFactory : IPlanetFactory
    {
        private GameObject planetPrefab;
        private IInstantiator instantiator;
        private IModuleFactory moduleFactory;
        private PlanetSelector selector;

        public CommonPlanetFactory(GameObject planetPrefab, IInstantiator instantiator, IModuleFactory moduleFactory,PlanetSelector selector)
        {
            this.planetPrefab = planetPrefab;
            this.instantiator = instantiator;
            this.moduleFactory = moduleFactory;
            this.selector = selector;
        }

        public GameObject CreatePlanet(PlanetData data)
        {
            GameObject planet = instantiator.InstantiatePrefab(planetPrefab);

            PlanetController controller = instantiator.InstantiateComponent<PlanetController>(planet);
            controller.SetPlanetData(data);
            foreach (KeyValuePair<string, ModuleData> valuePair in data.Modules)
            {
                moduleFactory.CreateModule(planet, valuePair.Value);
            }

            selector.AddPlanet(data.Guid, controller);
            selector.ForceSelect(controller);
            return planet;
        }
    }
}
