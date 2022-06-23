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

        protected bool IsToolEnabled;

        public abstract string DefaultKey { get; }

        public sealed override void DisableTool()
        {
            if (IsToolEnabled)
            {
                DoDisable();
                IsToolEnabled = false;
            }

        }

        public sealed override void EnableTool(InputSystem inputSystem)
        {
            if (!IsToolEnabled)
            {
                IsToolEnabled = true;
                DoEnable(inputSystem);
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

        protected virtual void DoEnable(InputSystem inputSystem)
        {
            TimeManager.Instance.LockTimeFlow(ToolName);
        }

        protected virtual void DoDisable()
        {
            TimeManager.Instance.UnlockTimeFlow();
        }

        public virtual void SwitchActiveState()
        {
            if(IsToolEnabled)
            {
                this.DisableTool();
            }
            else
            {
                this.toolsController.EnableTool(this.Key);
            }
        }
    }
}
