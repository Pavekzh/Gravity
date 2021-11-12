using System.Collections;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraZoomTool : EditorTool
    {
        [SerializeField] private float zoomSpeed = 1f;

        private float touchesDistance;

        private InputSystem inputSystem;

        public override void DisableTool()
        {
            if (inputSystem != null)
            {
                inputSystem.OnTwoTouchesDown -= this.TwoTouchesDown;
                inputSystem.OnTwoTouchesContinue -= this.ReedZoomInput;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            this.inputSystem = inputSystem;
            inputSystem.OnTwoTouchesDown += this.TwoTouchesDown;
            inputSystem.OnTwoTouchesContinue += this.ReedZoomInput; 
        }

        private void TwoTouchesDown(Touch[] touches)
        {
            Debug.Log("two touches");
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
            EditorController.Instance.Camera.Zoom(deltaDistance);

        }

    }
}