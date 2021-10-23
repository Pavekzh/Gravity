using BasicTools;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class CameraMovementTool : SceneTool
    {
        [SerializeField] private CameraZoomTool zoomTool;
        [SerializeField] private Vector2 movingSpeed = Vector2.one;

        private InputSystem inputSystem;

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
            EditorController.Instance.Camera.Moving((new Vector2(touch.deltaPosition.x,-touch.deltaPosition.y) * movingSpeed ).GetVector3());
        }
    }
}