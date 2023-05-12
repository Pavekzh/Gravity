using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public class EditorController : MonoBehaviour
    {
        [SerializeField] private InputSystem inputSystem;

        [SerializeField] private ToolsController toolsController;
        [SerializeField] private ManipulatorsController manipulatorsController;

        private PanelController panelToRestore;
        private PanelController panel;
       
        public ToolsController ToolsController { get => toolsController; }
        public ManipulatorsController ManipulatorsController { get => manipulatorsController; }


        public void OpenPanel(PanelController panel)
        {
            if(panel != this.panel && panel != null)
            {
                if (this.panel != null)
                {
                    if (this.panel.RestorePanel)
                        panel.RestorablePanel = this.panel;
                    else if(this.panel.RestorablePanel != null)
                    {
                        panel.RestorablePanel = this.panel.RestorablePanel;
                        this.panel.RestorablePanel = null;
                    }


                    this.panel.CloseWithoutRestore();
                }
                this.panel = panel;
            }
        }

        public void ClosePanel()
        {
            this.panel = null;
        }

        private void Start()
        {
            if (inputSystem == null)
            {
                Services.CommonMessagingSystem.Instance.ShowErrorMessage("Input system not set", this);
            }
            toolsController.Initialize(this.inputSystem);
            manipulatorsController.Initialize(this.inputSystem);
        }

    }
}