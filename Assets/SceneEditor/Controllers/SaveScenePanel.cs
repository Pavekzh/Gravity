using System.Collections;
using UnityEngine;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class SaveScenePanel : PanelController
    {
        [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private ShowElement visibleManager;

        public override void Open()
        {
            visibleManager.Show();
        }

        public override void Close()
        {
            visibleManager.Hide();
        }

        public void Save()
        {
            Assets.Services.SceneStateManager.Instance.SaveState(inputField.text);
            Close();
        }
    }
}