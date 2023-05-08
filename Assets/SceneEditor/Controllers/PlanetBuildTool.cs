using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SceneEditor.Models;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    class PlanetBuildTool : ObjectTool
    {
        [SerializeField] private MoveObjectTool MoveTool;

        public static string StaticKey => "BuildTool";

        public override string DefaultKey => StaticKey;
        public override string ToolName => "Object build tool";

        protected override bool HighlightSelectedObjectOnEnable => false;
        private InputSystem inputSystem;
        private SceneInstance scene;

        [Zenject.Inject]
        private void Construct(Services.SceneInstance scene)
        {
            this.scene = scene;
        }

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

            scene.AddPlanet(planetData.Clone() as PlanetData);

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
