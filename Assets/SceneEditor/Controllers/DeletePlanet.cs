using Assets.Services;
using System;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    class DeletePlanet:MonoBehaviour
    {
        private PlanetSelector selector;

        [Zenject.Inject]
        private void Construct(PlanetSelector selector)
        {
            this.selector = selector;
        }

        public void Delete()
        {            
            selector.SelectedPlanet.DeletePlanet();
        }
    }
}
