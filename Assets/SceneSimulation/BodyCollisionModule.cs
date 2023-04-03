using Assets.SceneEditor.Models;
using System;
using UnityEngine;
using Assets.SceneEditor.Controllers;

namespace Assets.SceneSimulation
{
    public class BodyCollisionModule : Module
    {
        LayerMask coreLayer;

        private void Start()
        {
            coreLayer = LayerMask.NameToLayer("Core");
        }

        public override ModuleData InstatiateModuleData()
        {
            return new BodyCollisionModuleData();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Services.TimeManager.Instance.SimulationState == true && other.gameObject.layer == coreLayer)
            {
                PlanetData planet1 = this.GetComponent<PlanetController>().PlanetData;                
                
                PlanetController controller2 = other.GetComponentInParent<PlanetController>();
                PlanetData planet2 = controller2.PlanetData;


                if (planet2 != null && planet1 != null)
                {
                    GravityModuleData gravity1 = planet1.GetModule<GravityModuleData>(GravityModuleData.Key);
                    GravityModuleData gravity2 = planet2.GetModule<GravityModuleData>(GravityModuleData.Key);                        
                    
                    ViewModuleData view1 = planet1.GetModule<ViewModuleData>(ViewModuleData.Key);
                    ViewModuleData view2 = planet2.GetModule<ViewModuleData>(ViewModuleData.Key);
                    if (gravity1.Mass > gravity2.Mass || (Mathf.Approximately(gravity1.Mass,gravity2.Mass) && view1.Volume >= view2.Volume))
                    {
                        Vector2 pulse = (gravity1.Mass * gravity1.Velocity) + (gravity2.Mass * gravity2.Velocity);
                        float mass = gravity1.Mass + gravity2.Mass;
                        Vector2 velocity = pulse / mass;
                        gravity1.Mass = mass;
                        gravity1.Velocity = velocity;
                        view1.Volume = view1.Volume + view2.Volume;

                        controller2.DeletePlanet();
                    }
                }
                else
                {
                    BasicTools.MessagingSystem.Instance.ShowErrorMessage("One or more planets has not PlanetController with set PlanetData [Line: 21]", this);
                }
            }

        }

    }
}
