using System.Collections;
using UnityEngine;
using UIExtended;
using Assets.Services;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public class SaveScenePanel : PanelController
    {
        [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private StateChanger visibleManager;

        private SceneStateLoader sceneLoader;

        [Zenject.Inject]
        private void Construct(SceneStateLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        protected override void DoOpen()
        {
            visibleManager.State = State.Changed;
        }

        protected override void DoClose()
        {
            visibleManager.State = State.Default;
        }

        public void Save()
        {
            this.RestorablePanel = null;
            sceneLoader.SaveState(inputField.text);
            Close();
        }
    }
}