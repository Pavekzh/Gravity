using Assets.SceneEditor.Models;
using System;
using UnityEngine;

namespace Assets.Services
{
    public class SceneInstance
    {
        private IPlanetFactory planetFactory;
        private Transform planetsParent;

        public SceneState CurrentScene { get; private set; }

        public event Action<SceneState,bool> SceneSet;
        public event Action<SceneState> SceneChanges;

        public SceneInstance(IPlanetFactory planetFactory,Transform planetParent)
        {
            this.planetFactory = planetFactory;
            this.planetsParent = planetParent;
        }

        public void ResetScene()
        {
            foreach (Transform planet in planetsParent)
            {
                planet.gameObject.SetActive(false);
                GameObject.Destroy(planet.gameObject);
            }
            CurrentScene = new SceneState();
            NotifySet(true);
        }

        public void ChangeScene(SceneState state,bool isValueLocal)
        {
            if (state != null)
            {
                ResetScene();
                foreach (PlanetData planetData in state.Planets)
                {
                    CreatePlanet(planetData);
                }
                CurrentScene = state;
                NotifySet(isValueLocal);
            }
        }        

        public void AddPlanet(PlanetData planetData)
        {
            CreatePlanet(planetData);
            CurrentScene.Planets.Add(planetData);
            NotifyChanges();
        }  

        public void RemovePlanet(PlanetData planetData)
        {
            CurrentScene.Planets.Remove(planetData);
            NotifyChanges();
        }

        private void NotifyChanges()
        {
            SceneChanges?.Invoke(CurrentScene);
        }

        private void NotifySet(bool isValueLocal)
        {
            SceneSet?.Invoke(CurrentScene, isValueLocal);
        }
        
        private void CreatePlanet(PlanetData data)
        {
            planetFactory.CreatePlanet(data).transform.parent = planetsParent;
        }
    }
}
