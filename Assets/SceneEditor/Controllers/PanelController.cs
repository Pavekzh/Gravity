using System.Collections;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public abstract class PanelController : MonoBehaviour
    {
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
                EditorController.Instance.OpenPanel(this);
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
                EditorController.Instance.ClosePanel();
                DoClose();
                IsOpened = false;
            }
        }

        protected abstract void DoOpen();
        protected abstract void DoClose();
    }
}