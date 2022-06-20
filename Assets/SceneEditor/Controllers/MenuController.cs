using System.Collections;
using UnityEngine;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class MenuController : PanelController
    {
        [SerializeField] ShowElement visibleManager;
        [SerializeField] LoadScenePanel loadScenePanel;
        [SerializeField] SaveScenePanel saveScenePanel;

        protected bool restorePanel = false;

        public override bool RestorePanel => restorePanel;

        protected override void DoOpen()
        {
            restorePanel = false;
            visibleManager.Show();
        }

        protected override void DoClose()
        {
            visibleManager.Hide();
        }

        public void ChangeVisibleState()
        {
            if (visibleManager.State == BasicTools.State.Default)
                Open();
            else if (visibleManager.State == BasicTools.State.Changed)
                Close();
        }

        public void Save()
        {
            Assets.Services.SceneStateManager.Instance.SaveState();
        }

        public void SaveAs()
        {
            this.restorePanel = true;
            saveScenePanel.Open();
        }

        public void Load()
        {
            this.restorePanel = true;
            loadScenePanel.Open();
        }

        public void Exit()
        {
            throw new System.NotImplementedException("Close editor not implemented");
        }
    }
}