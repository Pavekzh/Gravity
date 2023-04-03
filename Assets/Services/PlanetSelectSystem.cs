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
    public class PlanetSelectSystem : Singleton<PlanetSelectSystem> 
    {
        public delegate void SelectedPlanetChangedHandler(object sender, PlanetController planet);
        public event SelectedPlanetChangedHandler SelectedPlanetChanged;

        private int layerMask;
        private InputSystem inputSystem;
        private bool isSelectionLocked;
        private bool SelectedHighlighted;
        private Camera mainCamera;
       
        [SerializeField] private PlanetController planet;
        [SerializeField] private string planetsLayerName;
        [SerializeField] private float selectSphereRadius;

        private Dictionary<Guid, PlanetController> PlanetControllers { get; set; } = new Dictionary<Guid, PlanetController>();


        private bool mustBeSelected;

        public PlanetController SelectedPlanet
        {
            get => planet;
            private set
            {
                if (!isSelectionLocked)
                {
                    if (SelectedHighlighted)
                    {
                        mustBeSelected = true;                    
                        if (planet != null)
                            LessenSelected();
                        SelectedHighlighted = false;
                    }


                    planet = value;
                    SelectedPlanetChanged?.Invoke(this, value);
                    if (mustBeSelected && planet != null)
                    {
                        HighlightSelected();
                        mustBeSelected = false;
                    }
                }
            }
        }

        public void AddPlanet(Guid id, PlanetController planet)
        {
            PlanetControllers.Add(id, planet);
        }

        public void RemovePlanet(Guid id)
        {
            PlanetControllers.Remove(id);
            if(SelectedPlanet != null && id == SelectedPlanet.PlanetData.Guid)
                FindPlanet();
        }

        public void ForceSelect(PlanetController planet)
        {
            if(planet != null)
                this.SelectedPlanet = planet;
        }

        public void LockSelection()
        {
            isSelectionLocked = true;
        }

        public void UnlockSelection()
        {
            isSelectionLocked = false;
        }

        public void HighlightSelected()
        {
            if ((SelectedPlanet != null || FindPlanet() != null) && !SelectedHighlighted)
            {
                SelectedPlanet.PlanetData.GetModule<ViewModuleData>(ViewModuleData.Key).Highlight();
                SelectedHighlighted = true;
            }
           
        }

        public void LessenSelected()
        {
            if ((SelectedPlanet != null || FindPlanet() != null) && SelectedHighlighted)
            {
                SelectedPlanet.PlanetData.GetModule<ViewModuleData>(ViewModuleData.Key).Lessen();            
                SelectedHighlighted = false;
            }   

        }

        private void Start()
        {
            mainCamera = Camera.main;
            layerMask = 1 << LayerMask.NameToLayer(planetsLayerName);
            Services.SceneStateManager.Instance.SceneChanged += SceneChanged;
            if (planet == null)
                FindPlanet();
        }

        private void Update()
        {
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TouchDown(Input.GetTouch(0));
            }
        }

        private void SceneChanged()
        {
            if(SceneStateManager.Instance.CurrentScene.Planets.Count == 0)
            {
                PlanetControllers = new Dictionary<Guid, PlanetController>();
                SelectedPlanet = null;
            }
            else
                FindPlanet();
        }

        private PlanetController FindPlanet() 
        {
            if(PlanetControllers != null && PlanetControllers.Count != 0)
                SelectedPlanet = PlanetControllers.First().Value;
            return SelectedPlanet;
        }

        private void TouchDown(Touch touch)
        {
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            RaycastHit hitinfo;

            if (Physics.SphereCast(ray, selectSphereRadius, out hitinfo, Mathf.Infinity, layerMask))
            {
                PlanetController controller = hitinfo.collider.gameObject.GetComponent<PlanetController>();

                if (controller == null)
                {
                    Services.CommonMessagingSystem.Instance.ShowErrorMessage("SelectedObject must have Planet component", this);
                }
                else
                {
                    SelectedPlanet = controller;
                }
            }

        }
    }
}
