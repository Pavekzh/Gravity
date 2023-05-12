using System.Collections;
using UnityEngine;
using Assets.Services;
using UIExtended;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public class SettingsPanel : PanelController
    {
        [SerializeField] StateChanger visibleManager;
        [SerializeField] Services.AudioSettings settings;

        protected override void DoClose()
        {
            visibleManager.State = State.Default;
        }

        protected override void DoOpen()
        {
            visibleManager.State = State.Changed;
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