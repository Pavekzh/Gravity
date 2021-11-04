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

        public override void DisableTool()
        {                
            if(this.inputSystem != null)
            {
                this.inputSystem.OnTouchContinues -= Touch;
                this.inputSystem = null;
                EditorController.Instance.ToolsController.EnableSceneControl();
                Services.TimeManager.Instance.ResumePhysics();
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            if(this.inputSystem == null)
            {
                inputSystem.OnTouchContinues += Touch;
                this.inputSystem = inputSystem;
                EditorController.Instance.ToolsController.DisableSceneControl();
                Services.TimeManager.Instance.StopPhysics();
            }
        }

        private void Touch(Touch touch)
        {
            if(EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    EditorController.Instance.ToolsController.ObjectSelectionTool.SelectedPlanet.PlanetData.GetModule<GravityModuleData>(GravityModuleData.Key).Position = ray.GetPoint(distance).GetVectorXZ();
                }
            }
        }

    }
}
