using System;
using UnityEngine;
namespace Assets.SceneEditor.Controllers
{
    public abstract class SceneTool:EditorTool
    {
        [SerializeField] [Tooltip("Value used only for display state")] protected bool IsToolActive;

        private EditorController editor;

        [Zenject.Inject]
        private void Construct(EditorController editor)
        {
            this.editor = editor;
            editor.ToolsController.AddSceneTool(this);
        }
    }
}
