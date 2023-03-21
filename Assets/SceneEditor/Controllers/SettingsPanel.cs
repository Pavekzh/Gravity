using System.Collections;
using UnityEngine;
using Assets.Services;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class SettingsPanel : PanelController
    {
        [SerializeField] ShowElement visibleManager;
        [SerializeField] Services.AudioSettings settings;

        protected override void DoClose()
        {
            visibleManager.Hide();
        }

        protected override void DoOpen()
        {
            visibleManager.Show();
        }

        public void ToggleMusic()
        {
            settings.ToggleMusic();
        }

        public void ToggleSFX()
        {
            settings.ToggleSFX();
        }
    }
}