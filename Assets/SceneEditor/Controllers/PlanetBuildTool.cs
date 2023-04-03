using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    class PlanetBuildTool : ObjectTool
    {
        private InputSystem inputSystem;
        [SerializeField] private MoveObjectTool MoveTool;

        protected override bool HighlightSelectedObjectOnEnable => false;

        public override string DefaultKey => StaticKey;

        public static string StaticKey => "BuildTool";

        public override string ToolName => "Object build tool";

        protected override void DoDisable()
        {
            if(inputSystem != null)
            {
                inputSystem.OnTouchRelease -= StopCreating;
            }
        }

        protected override void DoEnable(InputSystem inputSystem)
        {
            inputSystem.OnTouchRelease += StopCreating;
            this.inputSystem = inputSystem;
        }

        public void Build(PlanetData planetData)
        {
            this.inputSystem.LockInputReading(true);
            PlanetController controller = (planetData.Clone() as PlanetData).CreateSceneObject();
            Services.PlanetSelectSystem.Instance.ForceSelect(controller);
            MoveTool.EnableTool(inputSystem);
            MoveTool.EnableImmediatelyInputReading();            
            MoveTool.DisableToolUI();
        }

        public void StopCreating(Touch touch)
        {
            MoveTool.DisableTool();

            this.inputSystem.UnlockInputReading();
            this.DisableTool();
        }
    }
}
