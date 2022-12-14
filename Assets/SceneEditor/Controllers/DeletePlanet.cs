using System;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    class DeletePlanet:MonoBehaviour
    {
        public void Delete()
        {
            Services.PlanetSelectSystem.Instance.SelectedPlanet.DeletePlanet();
        }
    }
}
