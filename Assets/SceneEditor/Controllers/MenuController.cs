using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public class MenuController : PanelController
    {
        [SerializeField] ShowElement visibleManager;
        [SerializeField] LoadScenePanel loadScenePanel;
        [SerializeField] SaveScenePanel saveScenePanel;
        [SerializeField] AudioSettingsPanel audioSettingPanel;
        [SerializeField] int menuSceneIndex;
        [SerializeField] LevelLoader levelLoader;

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

        public void Settings()
        {
            this.restorePanel = true;
            audioSettingPanel.Open();
        }

        public void Exit()
        {
            levelLoader.LoadLevel(menuSceneIndex);
        }
    }
}