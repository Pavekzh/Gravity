using BasicTools;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraMovementTool : SceneTool
    {
        [SerializeField] private new CameraModel camera;
        [SerializeField] private CameraZoomTool zoomTool;
        [SerializeField] private Vector2 movingSpeed = Vector2.one;

        private InputSystem inputSystem;

        public override string ToolName => "Camera movement tool";

        public override void DisableTool()
        {
            if (IsToolActive != false)
            {
                zoomTool.DisableTool();

                inputSystem.OnTouchContinues -= ReadMovingInput;
                inputSystem.OnTouchDown -= ReadMovingInput;
                inputSystem = null;

                this.IsToolActive = false;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            if(IsToolActive != true)
            {
                zoomTool.EnableTool(inputSystem);

                inputSystem.OnTouchContinues += ReadMovingInput;
                inputSystem.OnTouchDown += ReadMovingInput;                
                this.inputSystem = inputSystem;

                this.IsToolActive = true;
            }
        }

        private void ReadMovingInput(Touch touch)
        {
            camera.Moving((new Vector2(touch.deltaPosition.x,-touch.deltaPosition.y) * movingSpeed ).GetVector3());
        }
    }
}