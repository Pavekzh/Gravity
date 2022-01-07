using System.Collections;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraZoomTool : EditorTool
    {
        [SerializeField] private float zoomSpeed = 1f;

        private float touchesDistance;
        private bool isZooming;
        private InputSystem inputSystem;

        public override void DisableTool()
        {
            if (inputSystem != null)
            {
                inputSystem.OnTwoTouchesRelease -= this.ZoomStoped;
                inputSystem.OnTwoTouchesContinue -= this.ReedZoomInput;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            this.inputSystem = inputSystem;
            inputSystem.OnTwoTouchesRelease += this.ZoomStoped;
            inputSystem.OnTwoTouchesDown += this.ZoomStarted;
            inputSystem.OnTwoTouchesContinue += this.ReedZoomInput; 
        }

        private void ZoomStarted(Touch[] touches)
        {
            touchesDistance = Vector2.Distance(touches[0].position, touches[1].position);
            isZooming = true;
        }

        private void ZoomStoped(Touch[] touches)
        {
            touchesDistance = 0;
            isZooming = false;
        }

        private void ReedZoomInput(Touch[] touches)
        {
            if (isZooming)
            {
                float distance = Vector2.Distance(touches[0].position, touches[1].position);
                float deltaDistance = (distance - touchesDistance) * zoomSpeed;

                touchesDistance = distance;
                EditorController.Instance.Camera.Zoom(deltaDistance);
            }
        }

    }
}