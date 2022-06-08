using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    class PlanetBuildTool : ObjectTool
    {
        private InputSystem inputSystem;
        [SerializeField] private MovePlanetTool MovePlanetTool;

        public override string DefaultKey => StaticKey;

        public static string StaticKey => "BuildTool";

        public override void DisableTool()
        {
            if(inputSystem != null)
            {
                inputSystem.OnTouchRelease -= StopCreating;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            inputSystem.OnTouchRelease += StopCreating;
            this.inputSystem = inputSystem;
        }

        public void Build(PlanetData planetData)
        {
            this.inputSystem.IsInputEnabled = true;
            PlanetController controller = (planetData.Clone() as PlanetData).CreateSceneObject();
            Services.PlanetSelectSystem.Instance.ForceSelect(controller);
            MovePlanetTool.EnableTool(inputSystem);
        }

        public void StopCreating(Touch touch)
        {
            MovePlanetTool.DisableTool();
            this.DisableTool();
        }
    }
}
