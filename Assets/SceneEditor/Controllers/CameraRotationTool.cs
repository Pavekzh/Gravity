using System.Collections;
using UnityEngine;
using Assets.SceneEditor.Models;

namespace Assets.SceneEditor.Controllers
{
    public class CameraRotationTool : SceneTool
    {
        [SerializeField] private new CameraController camera;
        [SerializeField] private CameraZoomTool zoomTool;
        [SerializeField] private float rotationSpeed = 1;

        private InputSystem inputSystem;

        public override string ToolName => "Camera rotation tool";

        public override void DisableTool()
        {
            if(IsToolActive != false)
            {
                zoomTool.DisableTool();

                inputSystem.OnTouchDown -= ReadRotationInput;
                inputSystem.OnTouchContinues -= ReadRotationInput;
                inputSystem = null;

                IsToolActive = false;
            }
        }

        public override void EnableTool(InputSystem inputSystem)
        {
            if (IsToolActive != true)
            {
                zoomTool.EnableTool(inputSystem);

                inputSystem.OnTouchDown += ReadRotationInput;
                inputSystem.OnTouchContinues += ReadRotationInput;
                this.inputSystem = inputSystem;

                IsToolActive = true;
            }
        }

        private void ReadRotationInput(Touch touch)
        {
            Vector2 OrbitAngle = new Vector2();
            OrbitAngle.y = touch.deltaPosition.x * rotationSpeed;
            OrbitAngle.x = -(touch.deltaPosition.y * rotationSpeed);
            camera.Model.Rotation(OrbitAngle);
        }
    }
}