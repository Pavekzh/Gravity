using System.Collections;
using UnityEngine;

namespace Assets.Controllers
{
    public class CameraMovementTool : EditorTool
    {
        [SerializeField] private CameraZoomTool zoomTool;
        [SerializeField] private Vector3 movingSpeed = new Vector3(0.05f, 0.05f, 0.05f);

        private InputController inputSystem;
        public override void DisableTool()
        {
            zoomTool.DisableTool();
            inputSystem.OnTouchContinues -= ReadMovingInput;
            inputSystem.OnTouchDown -= ReadMovingInput;
        }

        public override void EnableTool(InputController inputSystem)
        {
            this.inputSystem = inputSystem;
            zoomTool.EnableTool(inputSystem);
            inputSystem.OnTouchContinues += ReadMovingInput;
            inputSystem.OnTouchDown += ReadMovingInput;
        }

        private void ReadMovingInput(Touch touch)
        {
            BasicTools.DataStorage.Instance.GetData<CameraModel>(CameraModel.Key).Moving(new Vector3(touch.deltaPosition.x * movingSpeed.x, 0, -(touch.deltaPosition.y * movingSpeed.z)));
        }

        public void SelectTool()
        {
            EditorController.Instance.SceneTool = this;
        }
    }
}