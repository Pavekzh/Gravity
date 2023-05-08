using System.Collections;
using UnityEngine;
using UIExtended;
using Assets.Services;

namespace Assets.SceneEditor.Controllers
{
    public class SaveScenePanel : PanelController
    {
        [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private ShowElement visibleManager;

        private SceneStateLoader sceneLoader;

        [Zenject.Inject]
        private void Construct(SceneStateLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        protected override void DoOpen()
        {
            visibleManager.Show();
        }

        protected override void DoClose()
        {
            visibleManager.Hide();
        }

        public void Save()
        {
            this.RestorablePanel = null;
            sceneLoader.SaveState(inputField.text);
            Close();
        }
    }
}