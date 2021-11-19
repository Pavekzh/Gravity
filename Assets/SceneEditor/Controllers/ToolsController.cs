using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class ToolsController:MonoBehaviour
    {
        public InputSystem InputSystem {get; set;}        
        public Dictionary<string, EditorTool> Tools { get; set; } = new Dictionary<string, EditorTool>();

        [SerializeField] private int selectedSceneTool = 0;

        private EditorTool sceneTool;
        private List<EditorTool> sceneManageTools = new List<EditorTool>();

        void Start()
        {
            SwitchSceneTool(selectedSceneTool);
        }

        public void DisableSceneControl()
        {
            sceneTool.DisableTool();
        }

        public void EnableSceneControl()
        {
            sceneTool.EnableTool(this.InputSystem);
        }

        public void SwitchSceneTool()
        {
            if (sceneTool != null)
                this.sceneTool.DisableTool();

            if (selectedSceneTool < sceneManageTools.Count - 1)
                sceneTool = sceneManageTools[selectedSceneTool += 1];
            else
                sceneTool = sceneManageTools[selectedSceneTool = 0];

            sceneTool.EnableTool(this.InputSystem);
        }

        public void SwitchSceneTool(int index)
        {
            if (sceneTool != null)
                this.sceneTool.DisableTool();

            if (index < sceneManageTools.Count)
                sceneTool = sceneManageTools[selectedSceneTool = index];
            else
                sceneTool = sceneManageTools[selectedSceneTool = 0];

            sceneTool.EnableTool(this.InputSystem);
        }

        public void AddSceneTool(EditorTool tool)
        {
            sceneManageTools.Add(tool);
        }

        public EditorTool EnableTool(string key)
        {
            EditorTool tool;
            if (Tools.TryGetValue(key, out tool))
            {
                if(objectTool != null)
                    objectTool.DisableTool();
                
                tool.EnableTool(InputSystem);
                objectTool = tool;

                return tool;
            }
            else
                BasicTools.ErrorManager.Instance.ShowErrorMessage("Tool was not found", this);
            return null;

        }

        private EditorTool objectTool;
    }
}
