using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    class MovePlanetTool : ObjectTool
    {
        private InputSystem inputSystem;

        public override string DefaultKey => "MoveTool";

        protected override void ForceDisableTool()
        {                
            if(this.inputSystem != null)
            {
                this.inputSystem.OnTouchContinues -= Touch;
                this.inputSystem = null;
                EditorController.Instance.ToolsController.EnableSceneControl();
            }
            base.ForceDisableTool();
        }

        protected override void ForceEnableTool(InputSystem inputSystem)
        {
            if(this.inputSystem == null)
            {
                inputSystem.OnTouchContinues += Touch;
                this.inputSystem = inputSystem;
                EditorController.Instance.ToolsController.DisableSceneControl();
            }
            base.ForceEnableTool(inputSystem);
        }

        private void Touch(Touch touch)
        {
            if(Services.PlanetSelectSystem.Instance.SelectedPlanet != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Services.PlanetSelectSystem.Instance.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).Position = ray.GetPoint(distance).GetVectorXZ();
                }
            }
        }

    }
}
