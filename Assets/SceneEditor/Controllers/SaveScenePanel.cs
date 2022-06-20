using System.Collections;
using UnityEngine;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class SaveScenePanel : PanelController
    {
        [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private ShowElement visibleManager;

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
            Assets.Services.SceneStateManager.Instance.SaveState(inputField.text);
            Close();
        }
    }
}