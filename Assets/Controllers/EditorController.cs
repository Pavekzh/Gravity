using System.Collections;
using UnityEngine;
using BasicTools;

namespace Assets.Controllers
{
    public class EditorController : Singleton<EditorController>
    {
        [SerializeField] InputController inputSystem;
        [SerializeField] private EditorTool sceneTool;
        [SerializeField] private EditorTool objectTool;
        [SerializeField] private PanelController panel;

        public EditorTool SceneTool
        {
            get => sceneTool;
            set
            {
                if(sceneTool != null)
                    sceneTool.DisableTool();

                sceneTool = value;
                sceneTool.EnableTool(inputSystem);
            }
        }
        public EditorTool ObjectTool
        {
            get => objectTool;
            set
            {
                if(objectTool != null)
                    objectTool.DisableTool();
                
                objectTool = value;
                objectTool.EnableTool(inputSystem);
            }
        }
        public PanelController Panel
        {
            get => panel;
            set
            {
                if(panel != null)
                    panel.Close();

                panel = value;
                panel.Open();
            }

        }

        private void Start()
        {
            if(inputSystem == null)
            {
                GenericErrorManager.Instance.ShowErrorMessage("Input system not set", this);
            }

            inputSystem.OnUITouch += LockInputControl;
            inputSystem.OnUIRelease += UnlockInputControl;
        }

        private void LockInputControl()
        {
            inputSystem.IsInputEnabled = false;
            Debug.Log("ControlLocked");
        }

        private void UnlockInputControl()
        {
            inputSystem.IsInputEnabled = true;
            Debug.Log("ControlUnlocked");
        }

    }
}