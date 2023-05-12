using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class MenuController : PanelController
    {
        [SerializeField] StateChanger visibleManager;
        [SerializeField] LoadScenePanel loadScenePanel;
        [SerializeField] SaveScenePanel saveScenePanel;
        [SerializeField] SettingsPanel audioSettingPanel;
        [SerializeField] int menuSceneIndex;
        [SerializeField] LevelLoader levelLoader;

        protected bool restorePanel = false;
        protected SceneStateLoader sceneLoader;

        public override bool RestorePanel => restorePanel;

        [Zenject.Inject]
        private void Construct(SceneStateLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        protected override void DoOpen()
        {
            restorePanel = false;
            visibleManager.State = State.Changed;
        }

        protected override void DoClose()
        {
            visibleManager.State = State.Default;
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
            sceneLoader.SaveState();
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