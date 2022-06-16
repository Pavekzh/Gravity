using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public class EditorController : Singleton<EditorController>
    {
        [SerializeField] private new Assets.SceneEditor.Models.CameraModel camera;
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private PanelController panel;
        [SerializeField] private ToolsController toolsController;

        public Models.CameraModel Camera { get => camera; set => camera = value; }
       
        public ToolsController ToolsController { get => toolsController; set => toolsController = value; }

        public PanelController Panel
        {
            get => panel;
            set
            {
                if(panel != null)
                    panel.Close();

                panel = value;
            }

        }

        private void Start()
        {
            if (inputSystem == null)
            {
                Services.CommonMessagingSystem.Instance.ShowErrorMessage("Input system not set", this);
            }

            inputSystem.OnUITouch += LockInputControl;
            inputSystem.OnUIRelease += UnlockInputControl;

            toolsController.InputSystem = this.inputSystem;
        }

        private void LockInputControl()
        {
            inputSystem.IsInputEnabled = false;
        }

        private void UnlockInputControl()
        {
            if(inputSystem.IsInputEnabled == false)
                inputSystem.IsInputEnabled = true;
        }

    }
}