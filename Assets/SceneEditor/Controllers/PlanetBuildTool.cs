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

        public override string ToolName => "Object build tool";

        protected override void ForceDisableTool()
        {
            if(inputSystem != null)
            {
                inputSystem.OnTouchRelease -= StopCreating;
            }
        }

        protected override void ForceEnableTool(InputSystem inputSystem)
        {
            inputSystem.OnTouchRelease += StopCreating;
            this.inputSystem = inputSystem;
        }

        public void Build(PlanetData planetData)
        {
            this.inputSystem.LockInputReading(true);
            PlanetController controller = (planetData.Clone() as PlanetData).CreateSceneObject();
            Services.PlanetSelectSystem.Instance.ForceSelect(controller);
            MovePlanetTool.EnableTool(inputSystem);
        }

        public void StopCreating(Touch touch)
        {
            MovePlanetTool.DisableTool();
            this.inputSystem.UnlockInputReading();
            this.DisableTool();
        }
    }
}
