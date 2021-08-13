using System.Collections;
using UnityEngine;

namespace Assets.Controllers
{
    public class CameraRotationTool : EditorTool
    {
        [SerializeField] private CameraZoomTool zoomTool;
        [SerializeField] private float rotationSpeed = 1;

        private InputController inputSystem;
        public override void DisableTool()
        {
            zoomTool.DisableTool();
            inputSystem.OnTouchDown -= ReadRotationInput;
            inputSystem.OnTouchContinues -= ReadRotationInput;
        }

        public override void EnableTool(InputController inputSystem)
        {
            this.inputSystem = inputSystem;
            zoomTool.EnableTool(inputSystem);
            inputSystem.OnTouchDown += ReadRotationInput;
            inputSystem.OnTouchContinues += ReadRotationInput;
        }

        private void ReadRotationInput(Touch touch)
        {
            Vector2 OrbitAngle = new Vector2();
            OrbitAngle.y = touch.deltaPosition.x * rotationSpeed;
            OrbitAngle.x = -(touch.deltaPosition.y * rotationSpeed);
            BasicTools.DataStorage.Instance.GetData<CameraModel>(CameraModel.Key).Rotation(OrbitAngle);
        }

        public void SelectTool()
        {
            EditorController.Instance.SceneTool = this;
        }
    }
}