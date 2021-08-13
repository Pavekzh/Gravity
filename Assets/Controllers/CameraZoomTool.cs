using System.Collections;
using UnityEngine;

namespace Assets.Controllers
{
    public class CameraZoomTool : EditorTool
    {
        [SerializeField] private CameraModel cameraManager;
        [SerializeField] private float zoomSpeed = 0.5f;

        public CameraModel CameraManager { get => cameraManager; set => cameraManager = value; }
        private float touchesDistance;

        private InputController inputSystem;

        public override void DisableTool()
        {
            inputSystem.OnTwoTouchesDown -= this.TwoTouchesDown;
            inputSystem.OnTwoTouchesContinue -= this.ReedZoomInput;
        }

        public override void EnableTool(InputController inputSystem)
        {
            this.inputSystem = inputSystem;
            inputSystem.OnTwoTouchesDown += this.TwoTouchesDown;
            inputSystem.OnTwoTouchesContinue += this.ReedZoomInput; 
        }

        private void TwoTouchesDown(Touch[] touches)
        {            
            float distance = 0;
            distance = Vector2.Distance(touches[0].position, touches[1].position);

            touchesDistance = distance;
        }

        private void ReedZoomInput(Touch[] touches)
        {
            float distance = 0;
            distance = Vector2.Distance(touches[0].position, touches[1].position);

            float deltaDistance = (distance - touchesDistance) * zoomSpeed;
            touchesDistance = distance;
            cameraManager.Zoom(deltaDistance);

        }

    }
}