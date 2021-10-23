using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class SelectPlanetTool : EditorTool
    {
        public delegate void SelectedPlanetChangedHandler(PlanetController planet, object sender);
        public event SelectedPlanetChangedHandler SelectedPlanetChanged;
        
        private int layerMask;
        private InputSystem inputSystem;

        [SerializeField]private PlanetController planet;
        [SerializeField]private string planetsLayerName;
        [SerializeField]private float selectSphereRadius;

        public PlanetController SelectedPlanet
        {
            get => planet;
            set
            {
                if(value != null)
                {
                    planet = value;
                    SelectedPlanetChanged?.Invoke(value, this);
                }
            }
        }

        void Start()
        {
            layerMask = 1 << LayerMask.NameToLayer(planetsLayerName);
            SelectedPlanet = Services.PlanetBuildSettings.Instance.PlanetsParent.GetComponentInChildren<PlanetController>();
        }   
        
        public override void DisableTool()
        {
            if (inputSystem != null)
            {
                inputSystem.OnTouchDown -= TouchDown;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            this.inputSystem = inputSystem;
            inputSystem.OnTouchDown += TouchDown;
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
