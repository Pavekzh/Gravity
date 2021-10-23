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


        public override void Open()
        {
            EditorController.Instance.Panel = this;
            visibleManager.Show();
        }

        public void ChangeVisibleState()
        {
            if (visibleManager.State == BasicTools.State.Default)
                Open();
            else if (visibleManager.State == BasicTools.State.Changed)
                Close();
        }

        public override void Close()
        {
            visibleManager.Hide();
        }

        public void Save()
        {
            Assets.Services.SceneStateManager.Instance.SaveState();
        }

        public void SaveAs()
        {
            EditorController.Instance.Panel = saveScenePanel;
            saveScenePanel.Open();
        }

        public void Load()
        {
            EditorController.Instance.Panel = loadScenePanel;
            loadScenePanel.Open();
        }

        public void Exit()
        {
            throw new System.NotImplementedException("Close editor not implemented");
        }
    }
}