using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public abstract class ObjectTool:EditorTool
    {
        [SerializeField] private string individualKey;
        [SerializeField] protected ToolsController toolsController;

        protected bool ToolSelectedAndWorking = false;
        protected bool IsToolEnabled;

        public abstract string DefaultKey { get; }

        public sealed override void DisableTool()
        {
            if (IsToolEnabled)
            {
                ForceDisableTool();
                IsToolEnabled = false;
            }

        }

        public sealed override void EnableTool(InputSystem inputSystem)
        {
            if (!IsToolEnabled)
            {
                IsToolEnabled = true;
                ForceEnableTool(inputSystem);
            }
        }

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

        protected virtual void ForceEnableTool(InputSystem inputSystem)
        {
            TimeManager.Instance.LockTimeFlow(ToolName);
        }

        protected virtual void ForceDisableTool()
        {
            TimeManager.Instance.UnlockTimeFlow();
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
