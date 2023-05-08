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

        protected virtual bool HighlightSelectedObjectOnEnable => true;
        protected bool IsToolEnabled;
        protected TimeFlow timeFlow;
        protected PlanetSelector selector;
        protected EditorController editor;

        public abstract string DefaultKey { get; }

        [Zenject.Inject]
        protected virtual void Construct(TimeFlow timeFlow,PlanetSelector selector,EditorController editor)
        {
            this.timeFlow = timeFlow;
            this.selector = selector;
            this.editor = editor;
            
            toolsController = editor.ToolsController;
            toolsController.Tools.Add(Key, this);
        }

        public sealed override void DisableTool()
        {
            if (IsToolEnabled)
            {
                DoDisable();
                IsToolEnabled = false;

                if (HighlightSelectedObjectOnEnable)
                    selector.LessenSelected();
            }

        }

        public sealed override void EnableTool(InputSystem inputSystem)
        {
            if (!IsToolEnabled)
            {
                IsToolEnabled = true;
                DoEnable(inputSystem);

                if (HighlightSelectedObjectOnEnable)
                    selector.HighlightSelected();
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
            this.DisableTool();
        }

        protected virtual void DoEnable(InputSystem inputSystem)
        {
            timeFlow.LockTimeFlow(ToolName);
        }

        protected virtual void DoDisable()
        {
           timeFlow.UnlockTimeFlow();
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
