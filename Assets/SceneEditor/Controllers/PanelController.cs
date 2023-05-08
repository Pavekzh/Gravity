using System.Collections;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public abstract class PanelController : MonoBehaviour
    {
        protected EditorController editor;

        /// <summary>
        /// Mark the panel as such that will automatically restored after closing the blocker panel
        /// </summary>
        public virtual bool RestorePanel { get; protected set; }
        public virtual bool IsOpened { get; protected set; }

        public PanelController RestorablePanel { get; set; }

        public void Open()
        {
            if (!IsOpened)
            {
                editor.OpenPanel(this);
                DoOpen();
                IsOpened = true;
            }

        }

        public void Close()
        {
            if (IsOpened)
            {
                CloseWithoutRestore();
                if(RestorablePanel != null && RestorablePanel.RestorePanel)
                    RestorablePanel.Open();
                RestorablePanel = null;
            }
        }

        public void CloseWithoutRestore()
        {
            if (IsOpened)
            {
                editor.ClosePanel();
                DoClose();
                IsOpened = false;
            }
        }

        [Zenject.Inject]
        protected virtual void Construct(EditorController editor)
        {
            this.editor = editor;
        }

        protected abstract void DoOpen();
        protected abstract void DoClose();
    }
}