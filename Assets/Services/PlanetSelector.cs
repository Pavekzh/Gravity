using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.SceneEditor.Controllers;
using Assets.Services;
using System.Linq;
using BasicTools;

namespace Assets.Services
{
    public class PlanetSelector : Singleton<PlanetSelector> 
    {
        public delegate void SelectedPlanetChangedHandler(object sender, PlanetController planet);
        public event SelectedPlanetChangedHandler SelectedPlanetChanged;

        private int layerMask;
        private InputSystem inputSystem;

        [SerializeField]private PlanetController planet;
        [SerializeField]private string planetsLayerName;
        [SerializeField]private float selectSphereRadius;

        public PlanetController SelectedPlanet
        {
            get => planet;
            private set
            {
                planet = value;
                SelectedPlanetChanged?.Invoke(this, value);
            }
        }

        public void ForceSelect(PlanetController planet)
        {
            if(planet != null)
                this.SelectedPlanet = planet;
        }

        private void Start()
        {
            layerMask = 1 << LayerMask.NameToLayer(planetsLayerName);
            Services.SceneStateManager.Instance.SceneChanged += SceneChanged;
            if (planet == null)
                FindPlanet();
        }

        private void FixedUpdate()
        {
            if(Input.touchCount == 1)
            {
                TouchDown(Input.GetTouch(0));
            }
        }

        private void SceneChanged()
        {
            if(SceneStateManager.Instance.CurrentScene.Planets.Count == 0)
                SelectedPlanet = null;
            else
                SelectedPlanet = FindPlanet();
        }

        private PlanetController FindPlanet()
        {
            if(Services.PlanetBuildSettings.Instance.PlanetsParent.childCount != 0)
            {


                planet = Services.PlanetBuildSettings.Instance.PlanetsParent.GetComponentInChildren<PlanetController>();
                if (planet != null)
                    SelectedPlanetChanged?.Invoke(this, planet);

                return planet;
            }
            return null;
        }

        private void TouchDown(Touch touch)
        {

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitinfo;
            if (Physics.SphereCast(ray, selectSphereRadius, out hitinfo, Mathf.Infinity, layerMask))
            {
                PlanetController controller = hitinfo.collider.gameObject.GetComponent<PlanetController>();

                if (controller == null)
                {
                    Services.CommonErrorManager.Instance.ShowErrorMessage("SelectedObject must have Planet component", this);
                }
                else
                {
                    SelectedPlanet = controller;
                }
            }

        }
    }
}
