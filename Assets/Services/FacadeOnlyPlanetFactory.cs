using System;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.Services
{
    class FacadeOnlyPlanetFactory : IPlanetFactory
    {
        private GameObject planetPrefab;
        private IInstantiator instantiator;
        private IModuleFactory moduleFactory;

        public FacadeOnlyPlanetFactory(GameObject planetPrefab, IInstantiator instantiator, IModuleFactory moduleFactory)
        {
            this.planetPrefab = planetPrefab;
            this.instantiator = instantiator;
            this.moduleFactory = moduleFactory;
        }

        public GameObject CreatePlanet(PlanetData data)
        {
            GameObject planet = instantiator.InstantiatePrefab(planetPrefab);

            foreach (KeyValuePair<string, ModuleData> valuePair in data.Modules)
            {
                if(valuePair.Key == ViewModuleData.Key)
                    moduleFactory.CreateModule(planet, valuePair.Value);
            }

            return planet;
        }
    }
}
