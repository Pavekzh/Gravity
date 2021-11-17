using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public abstract class ObjectTool:EditorTool
    {
        [SerializeField] private string individualKey;
        [SerializeField] protected ToolsController toolsController;

        protected bool ToolSelectedAndWorking = false;

        public abstract string DefaultKey { get; }
        
        public string Key
        {
            get
            {
                if (individualKey != "")
                    return individualKey;
                else
                    return DefaultKey;
            }
        }

        protected virtual void Awake()
        {
            if (toolsController == null)
                toolsController = EditorController.Instance.ToolsController;

            toolsController.Tools.Add(Key, this);
            this.DisableTool();
        }

        public virtual void SwitchActiveState()
        {
            if(ToolSelectedAndWorking)
            {
                this.DisableTool();
                ToolSelectedAndWorking = false;
            }
            else
            {
                this.toolsController.EnableTool(this.Key);
                ToolSelectedAndWorking = true;
            }
        }
    }
}
