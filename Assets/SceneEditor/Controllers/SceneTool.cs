using System;
using UnityEngine;
namespace Assets.SceneEditor.Controllers
{
    public abstract class SceneTool:EditorTool
    {
        [SerializeField] [Tooltip("Value used only for display state")] protected bool IsToolActive;

        protected virtual void Awake()
        {
            EditorController.Instance.ToolsController.AddSceneTool(this);
        }
    }
}
